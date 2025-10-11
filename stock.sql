-- SQLBook: Markup

-- ============================================
-- DATABASE CREATION
-- ============================================

-- CREATE DATABASE stock;
-- GO

-- USE stock;
-- GO

-- ============================================
-- LOOKUP AND BASE TABLES
-- ============================================

-- FormaPago Table
CREATE TABLE FormaPago (
    id_formaPago INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(45) NOT NULL
);

-- EstadoCompras Table
CREATE TABLE EstadoCompras (
    id_estadoCompras INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL -- Combines pendiente, aprobada, recibida, cancelada
);

-- EstadoVentas Table
CREATE TABLE EstadoVentas (
    id_estadoVentas INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(50) NOT NULL -- Combines facturada, entregada, cancelada
);

-- MotivoScrap Table
CREATE TABLE MotivoScrap (
    id_motivoScrap INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL -- Combines Daño, Vencido, Obsoleto, Mala calidad
);

-- CategoriasProducto Table
CREATE TABLE CategoriasProducto (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(45) NOT NULL,
    descripcion VARCHAR(100),
    estado VARCHAR(15)
);

-- MarcasProducto Table
CREATE TABLE MarcasProducto (
    id_marca INT PRIMARY KEY IDENTITY(1,1),
    marca VARCHAR(45) NOT NULL,
    estado VARCHAR(15)
);

-- Usuarios Table (Referenced in multiple tables, assuming it exists)
-- If it doesn't exist, create it. For this script, we assume it's present.
-- CREATE TABLE Usuarios (
--     id_usuario INT PRIMARY KEY IDENTITY(1,1),
--     nombre_usuario VARCHAR(50) NOT NULL,
--     ...
-- );

-- ============================================
-- CORE TABLES
-- ============================================

-- Productos Table
CREATE TABLE Productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    codBarras VARCHAR(20),
    nombre VARCHAR(45) NOT NULL,
    descripcion VARCHAR(100),
    id_marca INT,
    precioCompra DECIMAL(10, 2),
    precioVenta DECIMAL(10, 2),
    estado VARCHAR(10),
    ubicacion VARCHAR(45),
    habilitado BIT,
    id_categoria INT,
    FOREIGN KEY (id_marca) REFERENCES MarcasProducto(id_marca),
    FOREIGN KEY (id_categoria) REFERENCES CategoriasProducto(id_categoria)
);

-- Proveedores Table
CREATE TABLE Proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(45),
    razonSocial VARCHAR(100),
    CUIT VARCHAR(15),
    TiempoEntrega INT,
    Descuento DECIMAL(6, 2),
    id_formaPago INT,
    FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);

-- ProveedorTelefonos Table
CREATE TABLE ProveedorTelefonos (
    id_telefonoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    telefono VARCHAR(20),
    sector VARCHAR(45),
    horario VARCHAR(20),
    email VARCHAR(45),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- ProveedorUbicacion Table
CREATE TABLE ProveedorUbicacion (
    id_ubicacionProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    direccion VARCHAR(100),
    localidad VARCHAR(45),
    provincia VARCHAR(45),
    tipo VARCHAR(20), -- e.g., Sucursal, Depósito
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- ProductoProveedor Table
CREATE TABLE ProductoProveedor (
    id_productoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_proveedor INT,
    precioCompra DECIMAL(10, 2),
    tiempoEntrega INT,
    descuento DECIMAL(6, 2),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- Clientes Table
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(45),
    razonSocial VARCHAR(100),
    CUIT_DNI VARCHAR(15),
    id_formaPago INT, -- Changed to FK
    limiteCredito DECIMAL(10, 2),
    descuento DECIMAL(6, 2),
    estado VARCHAR(15),
    FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);

-- ClienteContactos Table
CREATE TABLE ClienteContactos (
    id_telefonoCliente INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    telefono VARCHAR(20),
    sector VARCHAR(45),
    horario VARCHAR(20),
    email VARCHAR(45),
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- ClienteDirecciones Table
CREATE TABLE ClienteDirecciones (
    id_direccion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    direccion VARCHAR(100),
    localidad VARCHAR(45),
    provincia VARCHAR(45),
    tipo VARCHAR(20),
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- ============================================
-- TRANSACTIONAL TABLES
-- ============================================

-- MovimientosStock Table
CREATE TABLE MovimientosStock (
    id_movimientosStock INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT,
    tipoMovimiento VARCHAR(20), -- e.g., Entrada, Salida
    fecha DATE,
    id_usuario INT, -- Assuming a Usuarios table exists
    id_producto INT,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
    -- FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
);

-- Stock Table
CREATE TABLE Stock (
    id_stock INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_usuario INT, -- Assuming a Usuarios table exists
    lote VARCHAR(20),
    stock INT,
    stockMinimo INT,
    stockIdeal INT,
    stockMaximo INT,
    tipoStock VARCHAR(20), -- e.g., Existencia, JIT
    puntoReposicion INT,
    fechaVencimiento DATE,
    estadoHabilitaciones VARCHAR(15),
    -- Removed incorrect FK to MovimientosStock, as it creates a circular dependency
    -- and is not logical. A stock entry is not a movement itself.
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
    -- FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
);

-- ScrapProducto Table
CREATE TABLE ScrapProducto (
    id_scrapProducto INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    cantidad INT,
    id_usuario INT, -- Assuming a Usuarios table exists
    fecha DATE,
    id_motivoScrap INT,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_motivoScrap) REFERENCES MotivoScrap(id_motivoScrap)
    -- FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
);

-- Compras Table
CREATE TABLE Compras (
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    tipoDocumento VARCHAR(30),
    numeroDocumento VARCHAR(20),
    montoTotal DECIMAL(10, 2),
    id_estadoCompras INT,
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_estadoCompras) REFERENCES EstadoCompras(id_estadoCompras)
);

-- DetalleCompras Table
CREATE TABLE DetalleCompras (
    id_detalleCompra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(10, 2),
    subtotal DECIMAL(10, 2),
    FOREIGN KEY (id_compra) REFERENCES Compras(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- Ventas Table
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    fecha DATE,
    tipoDocumento VARCHAR(30),
    numeroDocumento VARCHAR(20),
    montoTotal DECIMAL(10, 2),
    id_estadoVentas INT,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    FOREIGN KEY (id_estadoVentas) REFERENCES EstadoVentas(id_estadoVentas)
);

-- DetalleVentas Table
CREATE TABLE DetalleVentas (
    id_detalleVentas INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(10, 2),
    subtotal DECIMAL(10, 2),
    FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- ============================================
-- INSERT INITIAL DATA FOR LOOKUP TABLES
-- ============================================

INSERT INTO MotivoScrap (descripcion) VALUES
('Daño'),
('Vencido'),
('Obsoleto'),
('Mala calidad');

INSERT INTO EstadoCompras (descripcion) VALUES
('Pendiente'),
('Aprobada'),
('Recibida'),
('Cancelada');

INSERT INTO EstadoVentas (descripcion) VALUES
('Facturada'),
('Entregada'),
('Cancelada');

-- Example for FormaPago
INSERT INTO FormaPago (descripcion) VALUES
('Efectivo'),
('Tarjeta de Crédito'),
('Transferencia Bancaria');

GO