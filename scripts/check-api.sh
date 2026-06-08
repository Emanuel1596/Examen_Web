#!/usr/bin/env bash
set -e

BASE_URL="${BASE_URL:-http://localhost:5223}"

echo "Probando API en $BASE_URL"

ADMIN_LOGIN=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/login" -H "Content-Type: application/json" -d '{"email":"admin@miapp.com","password":"Admin123!"}')
ADMIN_BODY=$(echo "$ADMIN_LOGIN" | sed '$d')
ADMIN_STATUS=$(echo "$ADMIN_LOGIN" | tail -n 1)

if [ "$ADMIN_STATUS" != "200" ]; then
  echo "Falló login Admin. Código: $ADMIN_STATUS"
  echo "$ADMIN_BODY"
  exit 1
fi

ADMIN_TOKEN=$(echo "$ADMIN_BODY" | tr -d '\n' | sed -n 's/.*"token":"\([^"]*\)".*/\1/p')

if [ -z "$ADMIN_TOKEN" ]; then
  echo "No se pudo obtener el token Admin"
  echo "$ADMIN_BODY"
  exit 1
fi

READER_LOGIN=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/login" -H "Content-Type: application/json" -d '{"email":"reader@miapp.com","password":"Reader123!"}')
READER_BODY=$(echo "$READER_LOGIN" | sed '$d')
READER_STATUS=$(echo "$READER_LOGIN" | tail -n 1)

if [ "$READER_STATUS" != "200" ]; then
  echo "Falló login Reader. Código: $READER_STATUS"
  echo "$READER_BODY"
  exit 1
fi

READER_TOKEN=$(echo "$READER_BODY" | tr -d '\n' | sed -n 's/.*"token":"\([^"]*\)".*/\1/p')

if [ -z "$READER_TOKEN" ]; then
  echo "No se pudo obtener el token Reader"
  echo "$READER_BODY"
  exit 1
fi

INVALID_LOGIN=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/auth/login" -H "Content-Type: application/json" -d '{"email":"noexiste@miapp.com","password":"wrongpass"}')
INVALID_STATUS=$(echo "$INVALID_LOGIN" | tail -n 1)

if [ "$INVALID_STATUS" != "401" ]; then
  echo "Falló login inválido. Esperado: 401. Recibido: $INVALID_STATUS"
  exit 1
fi

ADMIN_USERS=$(curl -s -w "\n%{http_code}" "$BASE_URL/api/admin/users" -H "Authorization: Bearer $ADMIN_TOKEN")
ADMIN_USERS_STATUS=$(echo "$ADMIN_USERS" | tail -n 1)

if [ "$ADMIN_USERS_STATUS" != "200" ]; then
  echo "Falló ruta Admin users. Código: $ADMIN_USERS_STATUS"
  exit 1
fi

READER_PROFILE=$(curl -s -w "\n%{http_code}" "$BASE_URL/api/reader/profile" -H "Authorization: Bearer $READER_TOKEN")
READER_PROFILE_STATUS=$(echo "$READER_PROFILE" | tail -n 1)

if [ "$READER_PROFILE_STATUS" != "200" ]; then
  echo "Falló ruta Reader profile. Código: $READER_PROFILE_STATUS"
  exit 1
fi

DENIED=$(curl -s -w "\n%{http_code}" "$BASE_URL/api/admin/users" -H "Authorization: Bearer $READER_TOKEN")
DENIED_STATUS=$(echo "$DENIED" | tail -n 1)

if [ "$DENIED_STATUS" != "403" ]; then
  echo "Falló bloqueo Reader contra Admin. Esperado: 403. Recibido: $DENIED_STATUS"
  exit 1
fi

echo "API confirmada correctamente"
