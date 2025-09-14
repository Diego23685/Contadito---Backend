# Contadito.Api – Sprint 1 (Backend + Auth + Tenancy)

## Requisitos
- .NET 8 SDK
- MySQL 8+
- Node/Expo (cliente móvil)
- Postman/Insomnia para pruebas

## Configuración
1. Edita `appsettings.Development.json`:
   - `ConnectionStrings:Default`
   - `Jwt:Key` (usa un secreto largo y aleatorio).

2. Restaura paquetes y ejecuta:
```bash
dotnet restore
dotnet run
```

Swagger disponible en `https://localhost:5001/swagger` o `http://localhost:5000/swagger` según perfil.

## Endpoints clave
- `POST /auth/register-tenant` → crea tenant + usuario owner y devuelve JWT.
- `POST /auth/login` → devuelve JWT para el usuario.
- `GET /products` (autenticado) → paginado y filtrado por tenant.
- `POST /products`, `PUT /products/{id}`, `DELETE /products/{id}` (soft delete).
- `GET/POST/PUT/DELETE /customers`, `/warehouses`

## Tenancy
Middleware lee `tenant_id` del JWT y lo coloca en `HttpContext.Items["TenantId"]`. Todos los controladores usan ese valor para filtrar.

## Conectar con Expo
En Expo, añade `Authorization: Bearer <token>` a cada solicitud. Crea un usuario/tenant con `register-tenant` o usa `login`.

