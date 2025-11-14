# Woof – Aplicación móvil en .NET MAUI

**Woof** es una aplicación móvil desarrollada en **.NET MAUI**, orientada a la gestión básica de citas, mascotas, productos de tienda y veterinarios dentro de un consultorio veterinario.  
El proyecto utiliza **SQLite como base de datos local**, navegación por páginas y un diseño visual limpio y coherente.

---

## Características principales

### Pantalla de Login
- Validación de usuario con datos almacenados en SQLite  
- Usuario predeterminado:  
  - **Usuario:** admin  
  - **Contraseña:** 1234  
- Navegación a la pantalla principal después de una autenticación exitosa

### Pantalla Principal (Dashboard)
- Saludo personalizado según el usuario autenticado  
- Acceso directo a los módulos:
  - Citas
  - Mascotas
  - Tienda
  - Veterinarios
- Botón de **Cerrar Sesión** con mensaje de confirmación

---

## Módulos del sistema

Cada módulo incluye:  
- CRUD básico  
- Formularios dentro de tarjetas (Frame)
- Visualización en ListView  
- Eliminación por selección  
- Persistencia local con SQLite  

### Mascotas
Gestión de mascotas con los campos:  
- Nombre  
- Especie  
- Raza  
Incluye diseño basado en tarjetas visuales.

### Veterinarios
Administración de personal médico con los campos:  
- Nombre  
- Especialidad  
- Teléfono  
Incluye icono circular y tarjetas estilizadas.

### Tienda
Gestión de inventario simple con los campos:  
- Nombre  
- Precio  
- Stock  

### Citas
Registro y visualización de citas veterinarias con:
- Nombre de mascota  
- Motivo  
- Fecha seleccionada con DatePicker  

---

## Base de Datos – SQLite

El proyecto utiliza **sqlite-net-pcl**, configurado desde App.xaml.cs.  
Las tablas se crean automáticamente al iniciar la aplicación:

- Usuario  
- Mascota  
- Veterinario  
- ProductoTienda  
- Cita  

---

## Layout y Diseño

Tecnologías de interfaz utilizadas:
- VerticalStackLayout  
- Grid  
- Frame para tarjetas modernas  
- LinearGradientBrush para iconos  
- ScrollView para listas extensas  
- NavigationPage para navegación entre vistas  

### Estilo general
- Fondo suave (#F4F6FF)  
- Formularios dentro de tarjetas con esquinas redondeadas  
- Botones con colores sólidos y bordes redondeados  
- Tarjetas con sombras utilizando HasShadow="True"  
