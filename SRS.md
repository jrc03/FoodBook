# Software Requirements Specification (SRS)
## Plataforma de Reservas y Órdenes de Restaurantes – FoodBook Pro

---

## 1. Introducción

### 1.1 Propósito del documento
Este documento describe las especificaciones de requisitos de software (SRS) para la plataforma FoodBook Pro. El objetivo es definir de manera clara y precisa las funcionalidades y características requeridas para el sistema.

### 1.2 Alcance del sistema
FoodBook Pro es una plataforma web que permite a los usuarios buscar restaurantes por especialidades, realizar reservas, ordenar del menú, pagar en línea y dejar reseñas. También ofrece a los propietarios herramientas para gestionar su restaurante, su menú y visualizar estadísticas de las órdenes.

### 1.3 Definiciones, acrónimos y abreviaturas
- **SRS:** Software Requirements Specification
- **UI:** User Interface
- **API:** Application Programming Interface

---

## 2. Descripción General

### 2.1 Perspectiva del producto
El sistema será una aplicación web basada en arquitectura cliente-servidor desarrollada en .NET Core y React o Angular.

### 2.2 Funcionalidades del producto
- Búsqueda de restaurantes
- Reserva de mesas
- Pedido anticipado del menú
- Reseñas y evaluaciones
- Registro de usuarios y restaurantes
- Gestión de menú por propietarios
- Visualización de estadísticas
- Notificaciones en tiempo real
- Pasarela de pagos integrada
- Panel de administración

### 2.3 Características del usuario
- **Clientes:** Usuarios que hacen reservas, ordenan comida y escriben reseñas.
- **Propietarios:** Administran su restaurante, menú y estadísticas.
- **Administrador:** Supervisa la plataforma, gestiona contenido y reportes.

---

## 3. Requisitos Funcionales

### 3.1 Funcionalidades para Clientes
- **RF01:** Registro e inicio de sesión
- **RF02:** Búsqueda avanzada de restaurantes por especialidad, ubicación y evaluación
- **RF03:** Visualización de menú y disponibilidad de horarios
- **RF04:** Realización de reservas
- **RF05:** Realización de órdenes anticipadas
- **RF06:** Gestión de reseñas
- **RF07:** Recepción de notificaciones (confirmación o cancelación de reservas, estado de pedidos, promociones y recordatorios de visitas)
- **RF08:** Procesamiento de pagos

### 3.2 Funcionalidades para Propietarios
- **RF09:** Registro e inicio de sesión
- **RF10:** Gestión de perfil del restaurante
- **RF11:** Gestión del menú
- **RF12:** Recepción de notificaciones de órdenes y reservas
- **RF13:** Visualización de estadísticas de órdenes y actividad

### 3.3 Funcionalidades para Administradores
- **RF14:** Gestión de usuarios y restaurantes
- **RF15:** Monitoreo de actividad de la plataforma
- **RF16:** Generación de reportes globales
- **RF17:** Administración de contenido y categorías

---

## 4. Requisitos No Funcionales

- **RNF01 - Interfaz responsiva**
- **RNF02 - Seguridad en la autenticación y cifrado de datos**
- **RNF03 - Soporte para notificaciones en tiempo real**
- **RNF04 - Cumplimiento con estándares de pago (PCI-DSS)**
- **RNF05 - Alta disponibilidad**
- **RNF06 - Escalabilidad horizontal**
- **RNF07 - Sistema de auditoría y logs de transacciones**

---

## 5. Restricciones

- El prototipo inicial será solo web.
- Se utilizará .NET Core para backend.
- Frontend en React o Angular ASP.NET MVC Core.
- El sistema inicial no incluirá aplicación móvil.
