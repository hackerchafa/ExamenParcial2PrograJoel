# ExamenApi - API de Presupuestos

API REST para gestión de presupuestos mensuales y categorías de gastos.

## Despliegue en SOMEE

### Requisitos
- Cuenta en SOMEE (https://somee.com)
- Base de datos MySQL (Railway, PlanetScale, etc.)

### Pasos para desplegar

1. **Subir el código a GitHub**
   ```bash
   git add .
   git commit -m "Ready for SOMEE deployment"
   git push origin main
   ```

2. **En SOMEE:**
   - Crear nueva aplicación
   - Seleccionar "ASP.NET Core"
   - Conectar con tu repositorio de GitHub
   - Configurar variables de entorno (opcional)

3. **Variables de entorno recomendadas:**
   ```
   ConnectionStrings__DefaultConnection=tu_cadena_de_conexion_mysql
   Jwt__Key=tu_clave_jwt_secreta
   Jwt__Issuer=ExamenApi
   Jwt__Audience=ExamenApiUsers
   ```

### Endpoints disponibles

- `POST /api/auth/register` - Registro de usuarios
- `POST /api/auth/login` - Login de usuarios
- `GET /api/auth/token/validate` - Validar token JWT
- `GET /api/budgetcategory` - Obtener categorías de presupuesto
- `POST /api/budgetcategory` - Crear categoría de presupuesto
- `GET /api/budgetcategory/{id}` - Obtener categoría por ID
- `PUT /api/budgetcategory/{id}` - Actualizar categoría
- `DELETE /api/budgetcategory/{id}` - Eliminar categoría
- `POST /api/expenses` - Crear gasto
- `POST /api/budgets` - Crear presupuesto mensual
- `GET /api/budgets/{id}` - Obtener presupuesto por ID

### Tecnologías
- .NET 8
- Entity Framework Core
- MySQL (Pomelo.EntityFrameworkCore.MySql)
- JWT Authentication
- ASP.NET Core Identity 