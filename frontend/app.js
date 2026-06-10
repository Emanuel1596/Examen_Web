const apiBaseUrl = getApiBaseUrl();

let token = localStorage.getItem("token") || "";
let adminEventsCache = [];

document.addEventListener("DOMContentLoaded", () => {
  showPublic();

  if (token) {
    document.getElementById("login-box").classList.add("hidden");
    document.getElementById("admin-panel").classList.remove("hidden");
  }

  loadPublicEvents();
});

function getApiBaseUrl() {
  const host = window.location.host;

  if (host.includes(".app.github.dev")) {
    return `https://${host.replace("-3000.", "-5223.")}`;
  }

  return "http://localhost:5223";
}

function showPublic() {
  document.getElementById("public-section").classList.remove("hidden");
  document.getElementById("admin-section").classList.add("hidden");
  loadPublicEvents();
}

function showAdmin() {
  document.getElementById("public-section").classList.add("hidden");
  document.getElementById("admin-section").classList.remove("hidden");

  if (token) {
    document.getElementById("login-box").classList.add("hidden");
    document.getElementById("admin-panel").classList.remove("hidden");
    loadAdminEvents();
    loadDashboard();
  }
}

async function login() {
  const email = document.getElementById("email").value.trim();
  const password = document.getElementById("password").value;

  const response = await fetch(`${apiBaseUrl}/api/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ email, password })
  });

  if (!response.ok) {
    alert("No se pudo iniciar sesión. Revisa el correo y la contraseña.");
    return;
  }

  const data = await response.json();

  if (data.role !== "Admin") {
    alert("Solo el administrador puede entrar a esta sección.");
    return;
  }

  token = data.token;
  localStorage.setItem("token", token);

  document.getElementById("login-box").classList.add("hidden");
  document.getElementById("admin-panel").classList.remove("hidden");

  loadAdminEvents();
  loadDashboard();
}

function logout() {
  token = "";
  localStorage.removeItem("token");

  document.getElementById("login-box").classList.remove("hidden");
  document.getElementById("admin-panel").classList.add("hidden");

  showPublic();
}

async function loadPublicEvents() {
  const search = document.getElementById("public-search")?.value || "";
  const container = document.getElementById("public-events");

  try {
    const response = await fetch(`${apiBaseUrl}/api/events/active?search=${encodeURIComponent(search)}`);

    if (!response.ok) {
      container.innerHTML = `<p class="error">No se pudieron cargar los eventos.</p>`;
      return;
    }

    const events = await response.json();

    if (events.length === 0) {
      container.innerHTML = "<p>No hay eventos activos.</p>";
      return;
    }

    container.innerHTML = events.map(event => renderPublicEvent(event)).join("");
  } catch (error) {
    container.innerHTML = `<p class="error">Error al conectar con la API.</p>`;
  }
}

function renderPublicEvent(event) {
  if (!event.ticketZones || event.ticketZones.length === 0) {
    return `
      <div class="card">
        <h3>${escapeHtml(event.name)}</h3>
        <p>${escapeHtml(event.description)}</p>
        <p><strong>Fecha:</strong> ${formatDate(event.date)}</p>
        <p><strong>Lugar:</strong> ${escapeHtml(event.place)}</p>
        <p class="error">Este evento no tiene zonas configuradas.</p>
      </div>
    `;
  }

  const firstPrice = Number(event.ticketZones[0].price).toFixed(2);

  const zonesOptions = event.ticketZones.map(zone => {
    return `<option value="${zone.id}" data-price="${zone.price}">
      ${escapeHtml(zone.zone)} - $${Number(zone.price).toFixed(2)}
    </option>`;
  }).join("");

  return `
    <div class="card">
      <h3>${escapeHtml(event.name)}</h3>
      <p>${escapeHtml(event.description)}</p>
      <p><strong>Fecha:</strong> ${formatDate(event.date)}</p>
      <p><strong>Lugar:</strong> ${escapeHtml(event.place)}</p>

      <div class="zones">
        ${event.ticketZones.map(zone => `
          <div class="zone">${escapeHtml(zone.zone)}: $${Number(zone.price).toFixed(2)}</div>
        `).join("")}
      </div>

      <h4>Comprar boletos</h4>

      <label>Zona</label>
      <select id="zone-${event.id}" onchange="calculateTotal(${event.id})">
        ${zonesOptions}
      </select>

      <label>Cantidad</label>
      <input id="quantity-${event.id}" type="number" min="1" max="10" value="1" oninput="calculateTotal(${event.id})">

      <p><strong>Total:</strong> $<span id="total-${event.id}">${firstPrice}</span></p>

      <button onclick="purchaseTicket(${event.id})">Comprar</button>
    </div>
  `;
}

function calculateTotal(eventId) {
  const zoneSelect = document.getElementById(`zone-${eventId}`);
  const quantityInput = document.getElementById(`quantity-${eventId}`);
  const totalSpan = document.getElementById(`total-${eventId}`);

  if (!zoneSelect || !quantityInput || !totalSpan) {
    return;
  }

  const price = Number(zoneSelect.options[zoneSelect.selectedIndex].dataset.price);
  const quantity = Number(quantityInput.value);

  if (quantity <= 0) {
    totalSpan.textContent = "0.00";
    return;
  }

  totalSpan.textContent = (price * quantity).toFixed(2);
}

async function purchaseTicket(eventId) {
  const zoneSelect = document.getElementById(`zone-${eventId}`);
  const quantityInput = document.getElementById(`quantity-${eventId}`);

  const ticketZoneId = Number(zoneSelect.value);
  const quantity = Number(quantityInput.value);

  if (quantity <= 0) {
    alert("La cantidad debe ser mayor a 0.");
    return;
  }

  const response = await fetch(`${apiBaseUrl}/api/tickets/purchase`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      eventId,
      ticketZoneId,
      quantity
    })
  });

  if (!response.ok) {
    const error = await response.text();
    alert(`No se pudo realizar la compra: ${error}`);
    return;
  }

  const purchase = await response.json();

  alert(`Compra realizada correctamente. Total: $${Number(purchase.total).toFixed(2)}`);

  loadDashboard();
}

async function loadAdminEvents() {
  const search = document.getElementById("admin-search")?.value || "";
  const container = document.getElementById("admin-events");

  if (!token) {
    container.innerHTML = `<p class="error">Primero inicia sesión como administrador.</p>`;
    return;
  }

  try {
    const response = await fetch(`${apiBaseUrl}/api/events?search=${encodeURIComponent(search)}`, {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });

    if (!response.ok) {
      container.innerHTML = `<p class="error">No se pudieron cargar los eventos de administración.</p>`;
      return;
    }

    const events = await response.json();
    adminEventsCache = events;

    if (events.length === 0) {
      container.innerHTML = "<p>No hay eventos registrados.</p>";
      return;
    }

    container.innerHTML = events.map(event => renderAdminEvent(event)).join("");
  } catch (error) {
    container.innerHTML = `<p class="error">Error al conectar con la API.</p>`;
  }
}

function renderAdminEvent(event) {
  return `
    <div class="card">
      <h3>${escapeHtml(event.name)}</h3>
      <p>${escapeHtml(event.description)}</p>
      <p><strong>Fecha:</strong> ${formatDate(event.date)}</p>
      <p><strong>Lugar:</strong> ${escapeHtml(event.place)}</p>
      <p><strong>Estado:</strong> ${escapeHtml(event.status)}</p>

      <div class="zones">
        ${event.ticketZones.map(zone => `
          <div class="zone">${escapeHtml(zone.zone)}: $${Number(zone.price).toFixed(2)}</div>
        `).join("")}
      </div>

      <div class="actions">
        <button type="button" onclick="editEventById(${event.id})">Editar</button>
        <button type="button" class="danger" onclick="cancelEvent(${event.id})">Cancelar</button>
      </div>
    </div>
  `;
}

function editEventById(id) {
  const event = adminEventsCache.find(item => item.id === id);

  if (!event) {
    alert("No se encontró el evento para editar.");
    return;
  }

  document.getElementById("event-id").value = event.id;
  document.getElementById("event-name").value = event.name;
  document.getElementById("event-description").value = event.description;
  document.getElementById("event-date").value = toDateTimeLocal(event.date);
  document.getElementById("event-place").value = event.place;

  const vip = event.ticketZones.find(z => z.zone === "VIP");
  const preferente = event.ticketZones.find(z => z.zone === "Preferente");
  const general = event.ticketZones.find(z => z.zone === "General");

  document.getElementById("vip-price").value = vip ? vip.price : "";
  document.getElementById("preferente-price").value = preferente ? preferente.price : "";
  document.getElementById("general-price").value = general ? general.price : "";

  document.getElementById("event-name").focus();

  window.scrollTo({
    top: 0,
    behavior: "smooth"
  });
}

async function saveEvent() {
  const id = document.getElementById("event-id").value;

  const name = document.getElementById("event-name").value.trim();
  const description = document.getElementById("event-description").value.trim();
  const dateInput = document.getElementById("event-date").value;
  const place = document.getElementById("event-place").value.trim();
  const vipPrice = Number(document.getElementById("vip-price").value);
  const preferentePrice = Number(document.getElementById("preferente-price").value);
  const generalPrice = Number(document.getElementById("general-price").value);

  if (!name || !description || !dateInput || !place) {
    alert("Completa nombre, descripción, fecha y lugar.");
    return;
  }

  if (vipPrice <= 0 || preferentePrice <= 0 || generalPrice <= 0) {
    alert("Los precios deben ser mayores a 0.");
    return;
  }

  const eventData = {
    name,
    description,
    date: new Date(dateInput).toISOString(),
    place,
    vipPrice,
    preferentePrice,
    generalPrice
  };

  const url = id
    ? `${apiBaseUrl}/api/events/${id}`
    : `${apiBaseUrl}/api/events`;

  const method = id ? "PUT" : "POST";

  const response = await fetch(url, {
    method,
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify(eventData)
  });

  if (!response.ok) {
    const error = await response.text();
    alert(`No se pudo guardar el evento: ${error}`);
    return;
  }

  alert(id ? "Evento actualizado correctamente." : "Evento guardado correctamente.");

  clearForm();
  loadAdminEvents();
  loadPublicEvents();
  loadDashboard();
}

async function cancelEvent(id) {
  if (!confirm("¿Seguro que quieres cancelar este evento?")) {
    return;
  }

  const response = await fetch(`${apiBaseUrl}/api/events/${id}/cancel`, {
    method: "PATCH",
    headers: {
      "Authorization": `Bearer ${token}`
    }
  });

  if (!response.ok) {
    const error = await response.text();
    alert(`No se pudo cancelar el evento: ${error}`);
    return;
  }

  alert("Evento cancelado correctamente.");

  loadAdminEvents();
  loadPublicEvents();
  loadDashboard();
}

async function loadDashboard() {
  const container = document.getElementById("dashboard");

  if (!container || !token) {
    return;
  }

  const response = await fetch(`${apiBaseUrl}/api/dashboard/sales`, {
    headers: {
      "Authorization": `Bearer ${token}`
    }
  });

  if (!response.ok) {
    container.innerHTML = `<p class="error">No se pudo cargar el dashboard.</p>`;
    return;
  }

  const data = await response.json();

  container.innerHTML = `
    <div class="card">
      <p><strong>Compras:</strong> ${data.totalPurchases}</p>
      <p><strong>Boletos vendidos:</strong> ${data.totalTicketsSold}</p>
      <p><strong>Total vendido:</strong> $${Number(data.totalSales).toFixed(2)}</p>

      <h4>Ventas por evento</h4>

      ${data.salesByEvent.length === 0 ? "<p>Sin ventas registradas.</p>" : data.salesByEvent.map(item => `
        <div class="zone">
          ${escapeHtml(item.eventName)} - ${item.ticketsSold} boletos - $${Number(item.totalSales).toFixed(2)}
        </div>
      `).join("")}
    </div>
  `;
}

function clearForm() {
  document.getElementById("event-id").value = "";
  document.getElementById("event-name").value = "";
  document.getElementById("event-description").value = "";
  document.getElementById("event-date").value = "";
  document.getElementById("event-place").value = "";
  document.getElementById("vip-price").value = "";
  document.getElementById("preferente-price").value = "";
  document.getElementById("general-price").value = "";
}

function formatDate(value) {
  if (!value) {
    return "";
  }

  return new Date(value).toLocaleString("es-MX");
}

function toDateTimeLocal(value) {
  if (!value) {
    return "";
  }

  const date = new Date(value);

  if (Number.isNaN(date.getTime())) {
    return "";
  }

  const offset = date.getTimezoneOffset();
  const localDate = new Date(date.getTime() - offset * 60000);

  return localDate.toISOString().slice(0, 16);
}

function escapeHtml(value) {
  if (value === null || value === undefined) {
    return "";
  }

  return String(value)
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}
