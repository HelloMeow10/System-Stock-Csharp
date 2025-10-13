-- ============================================
-- TABLAS BASE
-- ============================================
CREATE TABLE MotivoScrap (
    id_motivoScrap INT IDENTITY(1,1) PRIMARY KEY,
    dano BIT DEFAULT 0,
    vencido BIT DEFAULT 0,
    obsoleto BIT DEFAULT 0,
    malaCalidad BIT DEFAULT 0
);

CREATE TABLE FormaPago (
    id_formaPago INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL
);

CREATE TABLE EstadoCompras (
    id_estadoCompras INT IDENTITY(1,1) PRIMARY KEY,
    pendiente BIT DEFAULT 0,
    aprobada BIT DEFAULT 0,
    recibida BIT DEFAULT 0,
    cancelada BIT DEFAULT 0
);

CREATE TABLE EstadoVentas (
    id_estadoVentas INT IDENTITY(1,1) PRIMARY KEY,
    facturada BIT DEFAULT 0,
    entregada BIT DEFAULT 0,  
    cancelada BIT DEFAULT 0
);

CREATE TABLE MarcasProducto (
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(15) DEFAULT 'Habilitado',
    marca VARCHAR(45) NOT NULL
);

CREATE TABLE CategoriasProducto (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(100) NOT NULL,
    descripcion VARCHAR(100),
    estado VARCHAR(15) DEFAULT 'Habilitado' 
);

-- ============================================
-- PRODUCTOS
-- ============================================

CREATE TABLE Productos (
    id_producto INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20) NOT NULL,
    codBarras VARCHAR(20),
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(200),
    id_marca INT,
    precioCompra DECIMAL(18,2),
    precioVenta DECIMAL(18,2),
    estado VARCHAR(25),
    ubicacion VARCHAR(100),
    habilitado BIT,
    id_categoria INT,
    FOREIGN KEY (id_marca) REFERENCES MarcasProducto(id_marca),
    FOREIGN KEY (id_categoria) REFERENCES CategoriasProducto(id_categoria)
);

-- ============================================
-- PROVEEDORES
-- ============================================
CREATE TABLE Proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT VARCHAR(20),
    TiempoEntrega INT,
    Descuento DECIMAL(5,2),
    id_formaPago INT,
    FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);

CREATE TABLE ProveedorTelefonos (
    id_telefonoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(50),
    email VARCHAR(100),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

CREATE TABLE ProveedorUbicacion (
    id_ubicacionProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    direccion VARCHAR(100),
    localidad VARCHAR(50),
    provincia VARCHAR(50),
    tipo VARCHAR(20), -- Sucursal / Depósito
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- Relación Productos - Proveedores
CREATE TABLE ProductoProveedor (
    id_productoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_proveedor INT,
    precioCompra DECIMAL(18,2),
    tiempoEntrega INT,
    descuento DECIMAL(5,2),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);

-- ============================================
-- COMPRAS
-- ============================================
CREATE TABLE Compras (
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    tipoDocumento VARCHAR(30), -- presupuesto / ordenCompra / Factura / notaDébito / notaCrédito
    numeroDocumento VARCHAR(20),
    montoTotal DECIMAL(18,2),
    id_estadoCompras INT,
    FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    FOREIGN KEY (id_estadoCompras) REFERENCES estadoCompras(id_estadoCompras)
);

CREATE TABLE DetalleCompras (
    id_detalleCompra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    FOREIGN KEY (id_compra) REFERENCES Compras(id_compra),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- ============================================
-- CLIENTES
-- ============================================
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT_DNI VARCHAR(20),
    formaPago VARCHAR(50),
    limiteCredito DECIMAL(18,2),
    descuento DECIMAL(5,2),
    estado VARCHAR(45)
);

CREATE TABLE ClienteContactos (
    id_telefonoCliente INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(20),
    email VARCHAR(100),
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

CREATE TABLE ClienteDirecciones (
    id_direccion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    direccion VARCHAR(100),
    localidad VARCHAR(45),
    provincia VARCHAR(45),
    tipo VARCHAR(20), -- Sucursal / Depósito
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);

-- ============================================
-- VENTAS
-- ============================================
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    fecha DATE,
    tipoDocumento VARCHAR(50),
    numeroDocumento VARCHAR(50),
    montoTotal DECIMAL(18,2),
    id_estadoVentas INT,
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    FOREIGN KEY (id_estadoVentas) REFERENCES EstadoVentas(id_estadoVentas)
);

CREATE TABLE DetalleVentas (
    id_detalleVentas INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

-- ============================================
-- STOCK Y MOVIMIENTOS
-- ============================================
CREATE TABLE MovimientosStock (
    id_movimientosStock INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT,
    tipoMovimiento VARCHAR(50),
    fecha DATE,
    id_usuario INT,
    id_producto INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);

CREATE TABLE Stock (
    id_stock INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_usuario INT,
    lote VARCHAR(50),
    stock INT,
    stockMinimo INT,
    stockIdeal INT,
    stockMaximo INT,
    tipoStock VARCHAR(20), -- Existencia o JIT
    puntoReposicion INT,
    fechaVencimiento DATE,
    estadoHabilitaciones VARCHAR(50),
    id_movimientosStock INT,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_movimientosStock) REFERENCES MovimientosStock(id_movimientosStock)
);

-- ============================================
-- SCRAP / BAJAS DE STOCK
-- ============================================
CREATE TABLE ScrapProducto (
    id_scrapProducto INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL, 
    cantidad INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    id_motivoScrap INT NOT NULL,
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    FOREIGN KEY (id_motivoScrap) REFERENCES MotivoScrap(id_motivoScrap)
);
