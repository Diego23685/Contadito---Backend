# Contadito API - Backend para PYMES (Sprint 1)

Contadito es una aplicacion multi-tenant para automatizar la gestion de costos, inventarios y ventas de pequenas y medianas empresas (PYMES).
Este backend esta construido con .NET 8 Web API y MySQL.

## Caracteristicas principales

- Multi-tenant: cada empresa (tenant) administra su propio inventario, clientes y almacenes.
- Autenticacion JWT: login seguro con roles (owner, admin, etc.).
- CRUDs basicos: productos, clientes y almacenes.
- Soft delete: usa DeletedAt en lugar de eliminar fisicamente los registros.
- Timestamps UTC: CreatedAt y UpdatedAt en todas las entidades.
- Serilog para logging.
- Preparado para crecer: futuras integraciones de facturacion, reportes y analisis.

## Tecnologias usadas

- .NET 8 Web API
- Entity Framework Core (Pomelo MySQL)
- JWT Bearer Authentication
- Serilog para logging
- MySQL para persistencia

## Configuracion

### Requisitos previos

- .NET SDK 8
- MySQL 8.x
- Postman o Curl para pruebas
- (Opcional) Docker para contenedores

### Variables y conexion

Edita appsettings.json o appsettings.Development.json:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=contadito;User Id=tu_usuario;Password=tu_password;SslMode=None;"
  },
  "Jwt": {
    "Key": "coloca_una_llave_muy_larga_y_segura",
    "Issuer": "Contadito",
    "Audience": "ContaditoClients",
    "ExpiresMinutes": 60
  }
}
```

## Instalacion y ejecucion

```bash
dotnet restore
dotnet run
```

Por defecto, la API estara disponible en:
http://localhost:5000

## Autenticacion

### Registrar un tenant
```bash
curl -X POST http://localhost:5000/auth/register-tenant -H "Content-Type: application/json" -d '{"tenantName":"DemoPyme","ownerName":"Owner","ownerEmail":"owner@demo.com","password":"pass123"}'
```

### Login
```bash
curl -X POST http://localhost:5000/auth/login -H "Content-Type: application/json" -d '{"email":"owner@demo.com","password":"pass123"}'
```
Respuesta:
```json
{ "token": "<JWT>", "expiresIn": 3600 }
```

Guarda el token para las siguientes llamadas.

## Endpoints disponibles

### Products
```bash
# Listar productos
curl -H "Authorization: Bearer <JWT>" http://localhost:5000/products

# Crear producto
curl -X POST http://localhost:5000/products -H "Authorization: Bearer <JWT>" -H "Content-Type: application/json" -d '{"sku":"SKU-001","name":"Producto Demo","description":"Descripcion","unit":"unidad","isService":false,"trackStock":true}'
```

### Customers
```bash
curl -H "Authorization: Bearer <JWT>" http://localhost:5000/customers
```

### Warehouses
```bash
curl -H "Authorization: Bearer <JWT>" http://localhost:5000/warehouses
```

## Estructura del proyecto

```
Contadito.Api/
 |- Controllers/          # Endpoints (Products, Customers, Warehouses, Auth)
 |- Data/                 # AppDbContext y configuracion EF Core
 |- Domain/
 |   |- DTOs/             # DTOs para requests/responses
 |   |- Entities/         # Entidades de base de datos
 |- Program.cs            # Configuracion de middleware, servicios y rutas
 |- appsettings.json      # Configuracion de conexion y JWT
 |- README.md             # Este archivo
```

## Notas de seguridad

- Usa HTTPS en produccion (dotnet dev-certs https --trust).
- Cambia la clave JWT por una segura y larga.
- Configura CORS si tu frontend Expo/React esta en otro dominio.
- Implementa control de roles para granularidad fina en sprints futuros.

## Roadmap

- Sprint 2: movimientos de inventario, estructura de costos y compras.
- Sprint 3: ventas, facturacion, pagos, reportes y benchmarking.

### Autor
Equipo Contadito - Backend inicial generado y documentado por Diego y colaboradores.
