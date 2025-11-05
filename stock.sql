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
    unidadesAvisoVencimiento INT DEFAULT 0,
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

CREATE TABLE DevolucionesProveedor (
    id_devolucion INT PRIMARY KEY IDENTITY(1,1),
    id_detalleCompra INT,
    motivo VARCHAR(100),
    fecha DATE DEFAULT GETDATE(),
    FOREIGN KEY (id_detalleCompra) REFERENCES DetalleCompras(id_detalleCompra)
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
    id_formaPago INT,
    limiteCredito DECIMAL(18,2),
    descuento DECIMAL(5,2),
    estado VARCHAR(45)
    FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
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


-- ============================================
-- STORES PROCEDURES
-- ============================================ 

-- almacenes, stock y scrap
CREATE PROCEDURE sp_IngresoMercaderia
    @id_producto INT,
    @id_usuario INT,
    @lote VARCHAR(50),
    @cantidad INT,
    @stockMinimo INT,
    @stockIdeal INT,
    @stockMaximo INT,
    @tipoStock VARCHAR(20),
    @puntoReposicion INT,
    @fechaVencimiento DATE,
    @estadoHabilitaciones VARCHAR(50),
    @id_movimientosStock INT
AS
BEGIN
    INSERT INTO Stock (id_producto, id_usuario, lote, stock, stockMinimo, stockIdeal, stockMaximo, tipoStock, puntoReposicion, fechaVencimiento, estadoHabilitaciones, id_movimientosStock)
    VALUES (@id_producto, @id_usuario, @lote, @cantidad, @stockMinimo, @stockIdeal, @stockMaximo, @tipoStock, @puntoReposicion, @fechaVencimiento, @estadoHabilitaciones, @id_movimientosStock)
END
GO

CREATE PROCEDURE sp_ReporteMovimientosStock
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT ms.id_movimientosStock,
           ms.fecha,
           u.nombre AS Usuario,
           p.nombre AS Producto,
           ms.tipoMovimiento,
           ms.cantidad
    FROM MovimientosStock ms
    INNER JOIN Usuarios u ON ms.id_usuario = u.id_usuario
    INNER JOIN Productos p ON ms.id_producto = p.id_producto
    WHERE ms.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

CREATE PROCEDURE sp_MoverStockAScrap
    @id_producto INT,
    @cantidad INT,
    @id_usuario INT,
    @id_motivoScrap INT
AS
BEGIN
    INSERT INTO ScrapProducto (id_producto, cantidad, id_usuario, id_motivoScrap)
    VALUES (@id_producto, @cantidad, @id_usuario, @id_motivoScrap)

    UPDATE Stock
    SET stock = stock - @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_ReporteScrap
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT sp.id_scrapProducto,
           sp.fecha,
           u.nombre AS Usuario,
           p.nombre AS Producto,
           ms.descripcion AS Motivo,
           sp.cantidad
    FROM ScrapProducto sp
    INNER JOIN Usuarios u ON sp.id_usuario = u.id_usuario
    INNER JOIN Productos p ON sp.id_producto = p.id_producto
    INNER JOIN MotivoScrap ms ON sp.id_motivoScrap = ms.id_motivoScrap
    WHERE sp.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

--clientes 
CREATE PROCEDURE sp_AgregarCliente
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuitDni VARCHAR(20),
    @id_formaPago INT,
    @limiteCredito DECIMAL(18,2),
    @descuento DECIMAL(5,2),
    @estado VARCHAR(45)
AS
BEGIN
    INSERT INTO Clientes (codigo, nombre, razonSocial, CUIT_DNI, id_formaPago, limiteCredito, descuento, estado)
    VALUES (@codigo, @nombre, @razonSocial, @cuitDni, @id_formaPago, @limiteCredito, @descuento, @estado)
END
GO

CREATE PROCEDURE sp_ModificarCliente
    @id_cliente INT,
    @codigo VARCHAR(20),
    @nombre VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuitDni VARCHAR(20),
    @id_formaPago INT,
    @limiteCredito DECIMAL(18,2),
    @descuento DECIMAL(5,2),
    @estado VARCHAR(45)
AS
BEGIN
    UPDATE Clientes
    SET codigo = @codigo,
        nombre = @nombre,
        razonSocial = @razonSocial,
        CUIT_DNI = @cuitDni,
        id_formaPago = @id_formaPago,
        limiteCredito = @limiteCredito,
        descuento = @descuento,
        estado = @estado
    WHERE id_cliente = @id_cliente
END
GO

CREATE PROCEDURE sp_EliminarCliente
    @id_cliente INT
AS
BEGIN
    DELETE FROM Clientes WHERE id_cliente = @id_cliente
END
GO

CREATE PROCEDURE sp_ConsultarClientePorNombre
    @nombre VARCHAR(50)
AS
BEGIN
    SELECT 
        id_cliente,
        codigo,
        nombre,
        razonSocial,
        CUIT_DNI,
        id_formaPago,
        limiteCredito,
        descuento,
        estado
    FROM Clientes
    WHERE nombre LIKE '%' + @nombre + '%' OR razonSocial LIKE '%' + @nombre + '%'
END
GO

CREATE PROCEDURE sp_ConsultarClientePorCUIT
    @cuitDni VARCHAR(20)
AS
BEGIN
    SELECT 
        id_cliente,
        codigo,
        nombre,
        razonSocial,
        CUIT_DNI,
        id_formaPago,
        limiteCredito,
        descuento,
        estado
    FROM Clientes
    WHERE CUIT_DNI LIKE '%' + @cuitDni + '%'
END
GO

CREATE PROCEDURE sp_AgregarTelefonoCliente
    @id_cliente INT,
    @telefono VARCHAR(20),
    @sector VARCHAR(50),
    @horario VARCHAR(20),
    @email VARCHAR(100)
AS
BEGIN
    INSERT INTO ClienteContactos (id_cliente, telefono, sector, horario, email)
    VALUES (@id_cliente, @telefono, @sector, @horario, @email)
END
GO

CREATE PROCEDURE sp_AgregarDireccionCliente
    @id_cliente INT,
    @direccion VARCHAR(100),
    @localidad VARCHAR(45),
    @provincia VARCHAR(45),
    @tipo VARCHAR(20)
AS
BEGIN
    INSERT INTO ClienteDirecciones (id_cliente, direccion, localidad, provincia, tipo)
    VALUES (@id_cliente, @direccion, @localidad, @provincia, @tipo)
END
GO

--compras y estados contables
CREATE PROCEDURE sp_RegistrarPresupuestoCompra
    @id_proveedor INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO PresupuestoCompra (id_proveedor, fecha, total)
    VALUES (@id_proveedor, @fecha, @total)
END
GO

CREATE PROCEDURE sp_RegistrarOrdenCompra
    @id_presupuesto INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO OrdenCompra (id_presupuesto, fecha, total)
    VALUES (@id_presupuesto, @fecha, @total)
END
GO

CREATE PROCEDURE sp_ConsultarPedidos
    @entregado BIT = NULL
AS
BEGIN
    SELECT 
        oc.id_ordenCompra,
        p.razonSocial AS proveedor,
        oc.fecha,
        oc.total,
        oc.entregado
    FROM OrdenCompra oc
    INNER JOIN Proveedor p ON oc.id_proveedor = p.id_proveedor
    WHERE (@entregado IS NULL OR oc.entregado = @entregado)
END
GO

CREATE PROCEDURE sp_RegistrarRemito
    @id_ordenCompra INT,
    @numeroRemito VARCHAR(50),
    @fecha DATE,
    @conFactura BIT
AS
BEGIN
    INSERT INTO Remito (id_ordenCompra, numeroRemito, fecha, conFactura)
    VALUES (@id_ordenCompra, @numeroRemito, @fecha, @conFactura)
END
GO

CREATE PROCEDURE sp_RegistrarFacturaCompra
    @id_proveedor INT,
    @id_remito INT,
    @numeroFactura VARCHAR(50),
    @fecha DATE,
    @total DECIMAL(18,2),
    @visadoAlmacen BIT
AS
BEGIN
    INSERT INTO FacturaCompra (id_proveedor, id_remito, numeroFactura, fecha, total, visadoAlmacen)
    VALUES (@id_proveedor, @id_remito, @numeroFactura, @fecha, @total, @visadoAlmacen)
END
GO

CREATE PROCEDURE sp_RegistrarNotaCredito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    INSERT INTO NotaCredito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

CREATE PROCEDURE sp_RegistrarNotaDebito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    INSERT INTO NotaDebito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

CREATE PROCEDURE sp_ReporteCompras
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL,
    @id_proveedor INT = NULL,
    @id_producto INT = NULL
AS
BEGIN
    SELECT 
        fc.id_factura,
        p.razonSocial AS proveedor,
        pr.nombre AS producto,
        fc.fecha,
        fc.total
    FROM FacturaCompra fc
    INNER JOIN Proveedor p ON fc.id_proveedor = p.id_proveedor
    INNER JOIN DetalleFacturaCompra df ON fc.id_factura = df.id_factura
    INNER JOIN Producto pr ON df.id_producto = pr.id_producto
    WHERE 
        (@fechaInicio IS NULL OR fc.fecha >= @fechaInicio) AND
        (@fechaFin IS NULL OR fc.fecha <= @fechaFin) AND
        (@id_proveedor IS NULL OR fc.id_proveedor = @id_proveedor) AND
        (@id_producto IS NULL OR pr.id_producto = @id_producto)
END
GO

CREATE PROCEDURE sp_ReporteDevolucionesProveedores
    @fechaInicio DATE = NULL,
    @fechaFin DATE = NULL
AS
BEGIN
    SELECT 
        d.id_devolucion,
        p.razonSocial AS proveedor,
        pr.nombre AS producto,
        d.fecha,
        d.motivo
    FROM DevolucionProveedor d
    INNER JOIN Proveedor p ON d.id_proveedor = p.id_proveedor
    INNER JOIN Producto pr ON d.id_producto = pr.id_producto
    WHERE 
        (@fechaInicio IS NULL OR d.fecha >= @fechaInicio) AND
        (@fechaFin IS NULL OR d.fecha <= @fechaFin)
END
GO

--productos
CREATE PROCEDURE sp_AgregarProducto
    @codigo VARCHAR(50),
    @codBarras VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255),
    @id_marca INT,
    @precioCompra DECIMAL(18,2),
    @precioVenta DECIMAL(18,2),
    @estado VARCHAR(50),
    @ubicacion VARCHAR(100),
    @habilitado BIT,
    @id_categoria INT
AS
BEGIN
    INSERT INTO Productos (codigo, codBarras, nombre, descripcion, id_marca, precioCompra, precioVenta, estado, ubicacion, habilitado, id_categoria)
    VALUES (@codigo, @codBarras, @nombre, @descripcion, @id_marca, @precioCompra, @precioVenta, @estado, @ubicacion, @habilitado, @id_categoria)
END
GO


CREATE PROCEDURE sp_ModificarProducto
    @id_producto INT,
    @codigo VARCHAR(50),
    @codBarras VARCHAR(50),
    @nombre VARCHAR(100),
    @descripcion VARCHAR(255),
    @id_marca INT,
    @precioCompra DECIMAL(18,2),
    @precioVenta DECIMAL(18,2),
    @estado VARCHAR(50),
    @ubicacion VARCHAR(100),
    @habilitado BIT,
    @id_categoria INT
AS
BEGIN
    UPDATE Productos
    SET codigo = @codigo,
        codBarras = @codBarras,
        nombre = @nombre,
        descripcion = @descripcion,
        id_marca = @id_marca,
        precioCompra = @precioCompra,
        precioVenta = @precioVenta,
        estado = @estado,
        ubicacion = @ubicacion,
        habilitado = @habilitado,
        id_categoria = @id_categoria
    WHERE id_producto = @id_producto
END
GO


CREATE PROCEDURE sp_EliminarProducto
    @id_producto INT
AS
BEGIN
    DELETE FROM Productos WHERE id_producto = @id_producto
END
GO


CREATE PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SELECT p.id_producto, p.codigo, p.nombre, p.descripcion, m.nombre AS Marca, c.categoria AS Categoria,
           p.precioCompra, p.precioVenta, p.estado, p.ubicacion, p.habilitado
    FROM Productos p
    LEFT JOIN Marcas m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
END
GO


CREATE PROCEDURE sp_BuscarProducto
    @busqueda VARCHAR(100)
AS
BEGIN
    SELECT p.*, m.nombre AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN Marcas m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @busqueda + '%'
       OR p.codigo LIKE '%' + @busqueda + '%'
       OR m.nombre LIKE '%' + @busqueda + '%'
       OR c.categoria LIKE '%' + @busqueda + '%'
END
GO


CREATE PROCEDURE sp_ProductosHabilitados
AS
BEGIN
    SELECT * FROM Productos WHERE habilitado = 1
END
GO


CREATE PROCEDURE sp_ProductosPorCategoria
    @id_categoria INT
AS
BEGIN
    SELECT * FROM Productos WHERE id_categoria = @id_categoria
END
GO


CREATE PROCEDURE sp_ProductosPorMarca
    @id_marca INT
AS
BEGIN
    SELECT * FROM Productos WHERE id_marca = @id_marca
END
GO


CREATE PROCEDURE sp_ConsultarProductoPorNombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT p.*, m.nombre AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN Marcas m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @nombre + '%'
END
GO


CREATE PROCEDURE sp_ConsultarProductoPorCodigo
    @codigo VARCHAR(50)
AS
BEGIN
    SELECT p.*, m.nombre AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN Marcas m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.codigo = @codigo
END
GO

--proveedores
CREATE PROCEDURE sp_AgregarProveedor
    @codigo VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuit VARCHAR(20),
    @email VARCHAR(100),
    @formaPago VARCHAR(100),
    @tiempoEntrega VARCHAR(50),
    @descuento DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Proveedor (codigo, razonSocial, cuit, email, formaPago, tiempoEntrega, descuento)
    VALUES (@codigo, @razonSocial, @cuit, @email, @formaPago, @tiempoEntrega, @descuento)
END
GO


CREATE PROCEDURE sp_ModificarProveedor
    @id_proveedor INT,
    @codigo VARCHAR(50),
    @razonSocial VARCHAR(100),
    @cuit VARCHAR(20),
    @email VARCHAR(100),
    @formaPago VARCHAR(100),
    @tiempoEntrega VARCHAR(50),
    @descuento DECIMAL(5,2)
AS
BEGIN
    UPDATE Proveedor
    SET codigo = @codigo,
        razonSocial = @razonSocial,
        cuit = @cuit,
        email = @email,
        formaPago = @formaPago,
        tiempoEntrega = @tiempoEntrega,
        descuento = @descuento
    WHERE id_proveedor = @id_proveedor
END
GO


CREATE PROCEDURE sp_EliminarProveedor
    @id_proveedor INT
AS
BEGIN
    DELETE FROM Proveedor WHERE id_proveedor = @id_proveedor
END
GO


CREATE PROCEDURE sp_ConsultarProveedoresPorNombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT 
        id_proveedor,
        codigo,
        razonSocial,
        cuit,
        email,
        formaPago,
        tiempoEntrega,
        descuento
    FROM Proveedor
    WHERE razonSocial LIKE '%' + @nombre + '%'
END
GO


CREATE PROCEDURE sp_ConsultarProveedoresPorCUIT
    @cuit VARCHAR(20)
AS
BEGIN
    SELECT 
        id_proveedor,
        codigo,
        razonSocial,
        cuit,
        email,
        formaPago,
        tiempoEntrega,
        descuento
    FROM Proveedor
    WHERE cuit LIKE '%' + @cuit + '%'
END
GO


CREATE PROCEDURE sp_AgregarTelefonoProveedor
    @id_proveedor INT,
    @contacto VARCHAR(100),
    @sector VARCHAR(100),
    @telefono VARCHAR(50),
    @email VARCHAR(100),
    @horario VARCHAR(100)
AS
BEGIN
    INSERT INTO TelefonoProveedor (id_proveedor, contacto, sector, telefono, email, horario)
    VALUES (@id_proveedor, @contacto, @sector, @telefono, @email, @horario)
END
GO


CREATE PROCEDURE sp_AgregarDireccionProveedor
    @id_proveedor INT,
    @direccion VARCHAR(255),
    @localidad VARCHAR(100),
    @provincia VARCHAR(100)
AS
BEGIN
    INSERT INTO DireccionProveedor (id_proveedor, direccion, localidad, provincia)
    VALUES (@id_proveedor, @direccion, @localidad, @provincia)
END
GO


CREATE PROCEDURE sp_RelacionarProductoProveedor
    @id_proveedor INT,
    @id_producto INT
AS
BEGIN
    INSERT INTO ProductoProveedor (id_proveedor, id_producto)
    VALUES (@id_proveedor, @id_producto)
END
GO


CREATE PROCEDURE sp_ConsultarProductosPorProveedor
    @id_proveedor INT
AS
BEGIN
    SELECT 
        p.id_producto,
        p.codigo,
        p.nombre,
        p.descripcion,
        p.precio
    FROM ProductoProveedor pp
    INNER JOIN Producto p ON pp.id_producto = p.id_producto
    WHERE pp.id_proveedor = @id_proveedor
END
GO


CREATE PROCEDURE sp_ConsultarProveedoresPorProducto
    @id_producto INT
AS
BEGIN
    SELECT 
        pr.id_proveedor,
        pr.razonSocial,
        pr.cuit,
        pr.formaPago,
        pr.descuento
    FROM ProductoProveedor pp
    INNER JOIN Proveedor pr ON pp.id_proveedor = pr.id_proveedor
    WHERE pp.id_producto = @id_producto
END
GO

-- TABLAS AUXILIARES: Categorías, Marcas, Formas de Pago, Estados

-- Categorías Producto
CREATE PROCEDURE sp_AgregarCategoria
    @categoria VARCHAR(100),
    @descripcion VARCHAR(100)
AS
BEGIN
    INSERT INTO CategoriasProducto (categoria, descripcion)
    VALUES (@categoria, @descripcion)
END
GO

CREATE PROCEDURE sp_ModificarCategoria
    @id_categoria INT,
    @categoria VARCHAR(100),
    @descripcion VARCHAR(100)
AS
BEGIN
    UPDATE CategoriasProducto
    SET categoria = @categoria,
        descripcion = @descripcion
    WHERE id_categoria = @id_categoria
END
GO

CREATE PROCEDURE sp_EliminarCategoria
    @id_categoria INT
AS
BEGIN
    DELETE FROM CategoriasProducto
    WHERE id_categoria = @id_categoria
END
GO

-- Marcas Producto
CREATE PROCEDURE sp_AgregarMarca
    @marca VARCHAR(45)
AS
BEGIN
    INSERT INTO MarcasProducto (marca)
    VALUES (@marca)
END
GO

CREATE PROCEDURE sp_ModificarMarca
    @id_marca INT,
    @marca VARCHAR(45)
AS
BEGIN
    UPDATE MarcasProducto
    SET marca = @marca
    WHERE id_marca = @id_marca
END
GO

CREATE PROCEDURE sp_EliminarMarca
    @id_marca INT
AS
BEGIN
    DELETE FROM MarcasProducto
    WHERE id_marca = @id_marca
END
GO

-- Formas de Pago
CREATE PROCEDURE sp_AgregarFormaPago
    @descripcion VARCHAR(100)
AS
BEGIN
    INSERT INTO FormaPago (descripcion)
    VALUES (@descripcion)
END
GO

CREATE PROCEDURE sp_ModificarFormaPago
    @id_formaPago INT,
    @descripcion VARCHAR(100)
AS
BEGIN
    UPDATE FormaPago
    SET descripcion = @descripcion
    WHERE id_formaPago = @id_formaPago
END
GO

CREATE PROCEDURE sp_EliminarFormaPago
    @id_formaPago INT
AS
BEGIN
    DELETE FROM FormaPago
    WHERE id_formaPago = @id_formaPago
END
GO

-- Estados Compras
CREATE PROCEDURE sp_ModificarEstadoCompra
    @id_estadoCompras INT,
    @pendiente BIT,
    @aprobada BIT,
    @recibida BIT,
    @cancelada BIT
AS
BEGIN
    UPDATE EstadoCompras
    SET pendiente = @pendiente,
        aprobada = @aprobada,
        recibida = @recibida,
        cancelada = @cancelada
    WHERE id_estadoCompras = @id_estadoCompras
END
GO

-- Estados Ventas
CREATE PROCEDURE sp_ModificarEstadoVenta
    @id_estadoVentas INT,
    @facturada BIT,
    @entregada BIT,
    @cancelada BIT
AS
BEGIN
    UPDATE EstadoVentas
    SET facturada = @facturada,
        entregada = @entregada,
        cancelada = @cancelada
    WHERE id_estadoVentas = @id_estadoVentas
END
GO


--VENTAS
CREATE PROCEDURE sp_AgregarPresupuestoVenta
    @id_cliente INT,
    @fecha DATE,
    @tipoDocumento VARCHAR(50),
    @numeroDocumento VARCHAR(50),
    @montoTotal DECIMAL(18,2),
    @id_estadoVentas INT
AS
BEGIN
    INSERT INTO Ventas (id_cliente, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoVentas)
    VALUES (@id_cliente, @fecha, @tipoDocumento, @numeroDocumento, @montoTotal, @id_estadoVentas)
END
GO

CREATE PROCEDURE sp_AgregarNotaPedido
    @id_venta INT,
    @id_producto INT,
    @cantidad INT,
    @precioUnitario DECIMAL(18,2),
    @subtotal DECIMAL(18,2)
AS
BEGIN
    INSERT INTO DetalleVentas (id_venta, id_producto, cantidad, precioUnitario, subtotal)
    VALUES (@id_venta, @id_producto, @cantidad, @precioUnitario, @subtotal)
    
    UPDATE Stock
    SET stock = stock - @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_AgregarNotaCredito
    @id_venta INT,
    @monto DECIMAL(18,2)
AS
BEGIN
    UPDATE Ventas
    SET montoTotal = montoTotal - @monto
    WHERE id_venta = @id_venta
END
GO

CREATE PROCEDURE sp_AgregarNotaDebito
    @id_venta INT,
    @monto DECIMAL(18,2)
AS
BEGIN
    UPDATE Ventas
    SET montoTotal = montoTotal + @monto
    WHERE id_venta = @id_venta
END
GO

CREATE PROCEDURE sp_DevolverProductoStock
    @id_producto INT,
    @cantidad INT
AS
BEGIN
    UPDATE Stock
    SET stock = stock + @cantidad
    WHERE id_producto = @id_producto
END
GO

CREATE PROCEDURE sp_ReporteVentas
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT v.id_venta,
           v.fecha,
           c.nombre AS Cliente,
           c.razonSocial AS RazonSocial,
           p.nombre AS Producto,
           cat.categoria AS Categoria,
           dv.cantidad,
           dv.precioUnitario,
           dv.subtotal,
           v.montoTotal
    FROM Ventas v
    INNER JOIN Clientes c ON v.id_cliente = c.id_cliente
    INNER JOIN DetalleVentas dv ON v.id_venta = dv.id_venta
    INNER JOIN Productos p ON dv.id_producto = p.id_producto
    INNER JOIN CategoriasProducto cat ON p.id_categoria = cat.id_categoria
    WHERE v.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO
