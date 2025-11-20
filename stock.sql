USE login2;
GO

-- ============================================
-- 🔹 LIMPIEZA AUTOMÁTICA DE OBJETOS EXISTENTES
-- ============================================

/* Solo dropeo procedures listados (si existen) para evitar conflicto al recrearlos */
DROP PROCEDURE IF EXISTS sp_IngresoMercaderia;
DROP PROCEDURE IF EXISTS sp_ReporteMovimientosStock;
DROP PROCEDURE IF EXISTS sp_MoverStockAScrap;
DROP PROCEDURE IF EXISTS sp_ReporteScrap;
DROP PROCEDURE IF EXISTS sp_AgregarCliente;
DROP PROCEDURE IF EXISTS sp_ModificarCliente;
DROP PROCEDURE IF EXISTS sp_EliminarCliente;
DROP PROCEDURE IF EXISTS sp_ConsultarClientePorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarClientePorCUIT;
DROP PROCEDURE IF EXISTS sp_AgregarTelefonoCliente;
DROP PROCEDURE IF EXISTS sp_AgregarDireccionCliente;
DROP PROCEDURE IF EXISTS sp_RegistrarPresupuestoCompra;
DROP PROCEDURE IF EXISTS sp_RegistrarOrdenCompra;
DROP PROCEDURE IF EXISTS sp_ConsultarPedidos;
DROP PROCEDURE IF EXISTS sp_RegistrarRemito;
DROP PROCEDURE IF EXISTS sp_RegistrarFacturaCompra;
DROP PROCEDURE IF EXISTS sp_RegistrarNotaCredito;
DROP PROCEDURE IF EXISTS sp_RegistrarNotaDebito;
DROP PROCEDURE IF EXISTS sp_ReporteCompras;
DROP PROCEDURE IF EXISTS sp_ReporteDevolucionesProveedores;
DROP PROCEDURE IF EXISTS sp_AgregarProducto;
DROP PROCEDURE IF EXISTS sp_ModificarProducto;
DROP PROCEDURE IF EXISTS sp_EliminarProducto;
DROP PROCEDURE IF EXISTS sp_ObtenerProductos;
DROP PROCEDURE IF EXISTS sp_BuscarProducto;
DROP PROCEDURE IF EXISTS sp_ProductosHabilitados;
DROP PROCEDURE IF EXISTS sp_ProductosPorCategoria;
DROP PROCEDURE IF EXISTS sp_ProductosPorMarca;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorCodigo;
DROP PROCEDURE IF EXISTS sp_AgregarProveedor;
DROP PROCEDURE IF EXISTS sp_ModificarProveedor;
DROP PROCEDURE IF EXISTS sp_EliminarProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorCUIT;
DROP PROCEDURE IF EXISTS sp_AgregarTelefonoProveedor;
DROP PROCEDURE IF EXISTS sp_AgregarDireccionProveedor;
DROP PROCEDURE IF EXISTS sp_RelacionarProductoProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProductosPorProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorProducto;
DROP PROCEDURE IF EXISTS sp_AgregarCategoria;
DROP PROCEDURE IF EXISTS sp_ModificarCategoria;
DROP PROCEDURE IF EXISTS sp_EliminarCategoria;
DROP PROCEDURE IF EXISTS sp_AgregarMarca;
DROP PROCEDURE IF EXISTS sp_ModificarMarca;
DROP PROCEDURE IF EXISTS sp_EliminarMarca;
DROP PROCEDURE IF EXISTS sp_AgregarFormaPago;
DROP PROCEDURE IF EXISTS sp_ModificarFormaPago;
DROP PROCEDURE IF EXISTS sp_EliminarFormaPago;
DROP PROCEDURE IF EXISTS sp_ModificarEstadoCompra;
DROP PROCEDURE IF EXISTS sp_ModificarEstadoVenta;
DROP PROCEDURE IF EXISTS sp_AgregarPresupuestoVenta;
DROP PROCEDURE IF EXISTS sp_AgregarNotaPedido;
DROP PROCEDURE IF EXISTS sp_AgregarNotaCredito;
DROP PROCEDURE IF EXISTS sp_AgregarNotaDebito;
DROP PROCEDURE IF EXISTS sp_DevolverProductoStock;
DROP PROCEDURE IF EXISTS sp_ReporteVentas;
GO

-- ============================================
-- TABLAS BASE
-- ============================================

IF OBJECT_ID('dbo.MotivoScrap','U') IS NULL
BEGIN
CREATE TABLE MotivoScrap (
    id_motivoScrap INT IDENTITY(1,1) PRIMARY KEY,
    dano BIT DEFAULT 0,
    vencido BIT DEFAULT 0,
    obsoleto BIT DEFAULT 0,
    malaCalidad BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.FormaPago','U') IS NULL
BEGIN
CREATE TABLE FormaPago (
    id_formaPago INT PRIMARY KEY IDENTITY(1,1),
    descripcion VARCHAR(100) NOT NULL
);
END
GO

IF OBJECT_ID('dbo.EstadoCompras','U') IS NULL
BEGIN
CREATE TABLE EstadoCompras (
    id_estadoCompras INT IDENTITY(1,1) PRIMARY KEY,
    pendiente BIT DEFAULT 0,
    aprobada BIT DEFAULT 0,
    recibida BIT DEFAULT 0,
    cancelada BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.EstadoVentas','U') IS NULL
BEGIN
CREATE TABLE EstadoVentas (
    id_estadoVentas INT IDENTITY(1,1) PRIMARY KEY,
    facturada BIT DEFAULT 0,
    entregada BIT DEFAULT 0,  
    cancelada BIT DEFAULT 0
);
END
GO

IF OBJECT_ID('dbo.MarcasProducto','U') IS NULL
BEGIN
CREATE TABLE MarcasProducto (
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(15) DEFAULT 'Habilitado',
    marca VARCHAR(45) NOT NULL
);
END
GO

IF OBJECT_ID('dbo.CategoriasProducto','U') IS NULL
BEGIN
CREATE TABLE CategoriasProducto (
    id_categoria INT PRIMARY KEY IDENTITY(1,1),
    categoria VARCHAR(100) NOT NULL,
    descripcion VARCHAR(100),
    estado VARCHAR(15) DEFAULT 'Habilitado' 
);
END
GO

-- ============================================
-- PRODUCTOS
-- ============================================
IF OBJECT_ID('dbo.Productos','U') IS NULL
BEGIN
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
    CONSTRAINT FK_Productos_Marcas FOREIGN KEY (id_marca) REFERENCES MarcasProducto(id_marca),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (id_categoria) REFERENCES CategoriasProducto(id_categoria)
);
END
GO

-- ============================================
-- PROVEEDORES
-- ============================================
IF OBJECT_ID('dbo.Proveedores','U') IS NULL
BEGIN
CREATE TABLE Proveedores (
    id_proveedor INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT VARCHAR(20),
    TiempoEntrega INT,
    Descuento DECIMAL(5,2),
    id_formaPago INT,
    CONSTRAINT FK_Proveedores_FormaPago FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);
END
GO

IF OBJECT_ID('dbo.ProveedorTelefonos','U') IS NULL
BEGIN
CREATE TABLE ProveedorTelefonos (
    id_telefonoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(50),
    email VARCHAR(100),
    CONSTRAINT FK_ProveedorTelefonos_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.ProveedorUbicacion','U') IS NULL
BEGIN
CREATE TABLE ProveedorUbicacion (
    id_ubicacionProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    direccion VARCHAR(100),
    localidad VARCHAR(50),
    provincia VARCHAR(50),
    tipo VARCHAR(20),
    CONSTRAINT FK_ProveedorUbicacion_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

IF OBJECT_ID('dbo.ProductoProveedor','U') IS NULL
BEGIN
CREATE TABLE ProductoProveedor (
    id_productoProveedor INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_proveedor INT,
    precioCompra DECIMAL(18,2),
    tiempoEntrega INT,
    descuento DECIMAL(5,2),
    CONSTRAINT FK_ProductoProveedor_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_ProductoProveedor_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor)
);
END
GO

-- ============================================
-- COMPRAS
-- ============================================
IF OBJECT_ID('dbo.Compras','U') IS NULL
BEGIN
CREATE TABLE Compras (
    id_compra INT PRIMARY KEY IDENTITY(1,1),
    id_proveedor INT,
    fecha DATE,
    tipoDocumento VARCHAR(30),
    numeroDocumento VARCHAR(20),
    montoTotal DECIMAL(18,2),
    id_estadoCompras INT,
    CONSTRAINT FK_Compras_Proveedor FOREIGN KEY (id_proveedor) REFERENCES Proveedores(id_proveedor),
    CONSTRAINT FK_Compras_EstadoCompras FOREIGN KEY (id_estadoCompras) REFERENCES EstadoCompras(id_estadoCompras)
);
END
GO

IF OBJECT_ID('dbo.DetalleCompras','U') IS NULL
BEGIN
CREATE TABLE DetalleCompras (
    id_detalleCompra INT PRIMARY KEY IDENTITY(1,1),
    id_compra INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    CONSTRAINT FK_DetalleCompras_Compras FOREIGN KEY (id_compra) REFERENCES Compras(id_compra),
    CONSTRAINT FK_DetalleCompras_Productos FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.DevolucionesProveedor','U') IS NULL
BEGIN
CREATE TABLE DevolucionesProveedor (
    id_devolucion INT PRIMARY KEY IDENTITY(1,1),
    id_detalleCompra INT,
    motivo VARCHAR(100),
    fecha DATE DEFAULT GETDATE(),
    CONSTRAINT FK_Devoluciones_DetalleCompras FOREIGN KEY (id_detalleCompra) REFERENCES DetalleCompras(id_detalleCompra)
);
END
GO

-- ============================================
-- CLIENTES
-- ============================================
IF OBJECT_ID('dbo.Clientes','U') IS NULL
BEGIN
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY(1,1),
    codigo VARCHAR(20),
    nombre VARCHAR(50),
    razonSocial VARCHAR(100),
    CUIT_DNI VARCHAR(20),
    id_formaPago INT,
    limiteCredito DECIMAL(18,2),
    descuento DECIMAL(5,2),
    estado VARCHAR(45),
    CONSTRAINT FK_Clientes_FormaPago FOREIGN KEY (id_formaPago) REFERENCES FormaPago(id_formaPago)
);
END
GO

IF OBJECT_ID('dbo.ClienteContactos','U') IS NULL
BEGIN
CREATE TABLE ClienteContactos (
    id_telefonoCliente INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    telefono VARCHAR(20),
    sector VARCHAR(50),
    horario VARCHAR(20),
    email VARCHAR(100),
    CONSTRAINT FK_ClienteContactos_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);
END
GO

IF OBJECT_ID('dbo.ClienteDirecciones','U') IS NULL
BEGIN
CREATE TABLE ClienteDirecciones (
    id_direccion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    direccion VARCHAR(100),
    localidad VARCHAR(45),
    provincia VARCHAR(45),
    tipo VARCHAR(20),
    CONSTRAINT FK_ClienteDirecciones_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente)
);
END
GO

-- ============================================
-- VENTAS
-- ============================================
IF OBJECT_ID('dbo.Ventas','U') IS NULL
BEGIN
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT,
    fecha DATE,
    tipoDocumento VARCHAR(50),
    numeroDocumento VARCHAR(50),
    montoTotal DECIMAL(18,2),
    id_estadoVentas INT,
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    CONSTRAINT FK_Ventas_EstadoVentas FOREIGN KEY (id_estadoVentas) REFERENCES EstadoVentas(id_estadoVentas)
);
END
GO

IF OBJECT_ID('dbo.DetalleVentas','U') IS NULL
BEGIN
CREATE TABLE DetalleVentas (
    id_detalleVentas INT PRIMARY KEY IDENTITY(1,1),
    id_venta INT,
    id_producto INT,
    cantidad INT,
    precioUnitario DECIMAL(18,2),
    subtotal DECIMAL(18,2),
    CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (id_venta) REFERENCES Ventas(id_venta),
    CONSTRAINT FK_DetalleVentas_Productos FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

-- ============================================
-- STOCK Y MOVIMIENTOS
-- ============================================
IF OBJECT_ID('dbo.MovimientosStock','U') IS NULL
BEGIN
CREATE TABLE MovimientosStock (
    id_movimientosStock INT PRIMARY KEY IDENTITY(1,1),
    cantidad INT,
    tipoMovimiento VARCHAR(50),
    fecha DATE,
    id_usuario INT,
    id_producto INT,
    CONSTRAINT FK_MovStock_Usuarios FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_MovStock_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
);
END
GO

IF OBJECT_ID('dbo.Stock','U') IS NULL
BEGIN
CREATE TABLE Stock (
    id_stock INT PRIMARY KEY IDENTITY(1,1),
    id_producto INT,
    id_usuario INT,
    lote VARCHAR(50),
    stock INT,
    stockMinimo INT,
    stockIdeal INT,
    stockMaximo INT,
    tipoStock VARCHAR(20),
    puntoReposicion INT,
    fechaVencimiento DATE,
    estadoHabilitaciones VARCHAR(50),
    id_movimientosStock INT,
    CONSTRAINT FK_Stock_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_Stock_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_Stock_MovStock FOREIGN KEY (id_movimientosStock) REFERENCES MovimientosStock(id_movimientosStock)
);
END
GO

-- ============================================
-- SCRAP / BAJAS DE STOCK
-- ============================================
IF OBJECT_ID('dbo.ScrapProducto','U') IS NULL
BEGIN
CREATE TABLE ScrapProducto (
    id_scrapProducto INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL, 
    cantidad INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    id_motivoScrap INT NOT NULL,
    CONSTRAINT FK_Scrap_Producto FOREIGN KEY (id_producto) REFERENCES Productos(id_producto),
    CONSTRAINT FK_Scrap_Usuario FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario),
    CONSTRAINT FK_Scrap_Motivo FOREIGN KEY (id_motivoScrap) REFERENCES MotivoScrap(id_motivoScrap)
);
END
GO

-- Vistas para compatibilidad con nombres usados en procedures (no modificamos logic)
IF OBJECT_ID('dbo.Producto','V') IS NOT NULL DROP VIEW dbo.Producto;
GO
CREATE VIEW dbo.Producto
AS
SELECT id_producto, codigo, codBarras, nombre, descripcion, id_marca, precioCompra, precioVenta, estado, ubicacion, habilitado, id_categoria
FROM dbo.Productos;
GO

IF OBJECT_ID('dbo.Proveedor','V') IS NOT NULL DROP VIEW dbo.Proveedor;
GO
CREATE VIEW dbo.Proveedor
AS
SELECT id_proveedor, codigo, nombre, razonSocial, CUIT, TiempoEntrega, Descuento, id_formaPago
FROM dbo.Proveedores;
GO

-- FacturaCompra y DetalleFacturaCompra son nombres que aparecen en SPs; mapearlos a Compras/DetalleCompras
IF OBJECT_ID('dbo.FacturaCompra','V') IS NOT NULL DROP VIEW dbo.FacturaCompra;
GO
CREATE VIEW dbo.FacturaCompra
AS
SELECT id_compra AS id_factura, id_proveedor, fecha, numeroDocumento AS numeroFactura, tipoDocumento, montoTotal AS total, id_estadoCompras
FROM dbo.Compras;
GO

IF OBJECT_ID('dbo.DetalleFacturaCompra','V') IS NOT NULL DROP VIEW dbo.DetalleFacturaCompra;
GO
CREATE VIEW dbo.DetalleFacturaCompra
AS
SELECT id_detalleCompra AS id_detalleFactura, id_compra AS id_factura, id_producto, cantidad, precioUnitario, subtotal
FROM dbo.DetalleCompras;
GO

-- ProductoProveedor alias (por si los SPs usan Producto/Proveedor)
IF OBJECT_ID('dbo.ProductoProveedor_V','V') IS NOT NULL DROP VIEW dbo.ProductoProveedor_V;
GO
CREATE VIEW dbo.ProductoProveedor_V
AS
SELECT id_productoProveedor, id_producto, id_proveedor, precioCompra, tiempoEntrega, descuento
FROM dbo.ProductoProveedor;
GO

-- ============================================
-- STORES PROCEDURES
-- ============================================ 

-- almacenes, stock y scrap
DROP PROCEDURE IF EXISTS sp_IngresoMercaderia;
GO
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

DROP PROCEDURE IF EXISTS sp_ReporteMovimientosStock;
GO
CREATE PROCEDURE sp_ReporteMovimientosStock
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT ms.id_movimientosStock,
           ms.fecha,
           per.nombre AS Usuario,
           prod.nombre AS Producto,
           ms.tipoMovimiento,
           ms.cantidad
    FROM MovimientosStock ms
    INNER JOIN Usuarios u ON ms.id_usuario = u.id_usuario
    INNER JOIN personas per ON u.id_persona = per.id_persona
    INNER JOIN Productos prod ON ms.id_producto = prod.id_producto
    WHERE ms.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

DROP PROCEDURE IF EXISTS sp_MoverStockAScrap;
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

DROP PROCEDURE IF EXISTS sp_ReporteScrap;
GO
CREATE PROCEDURE sp_ReporteScrap
    @fechaDesde DATE,
    @fechaHasta DATE
AS
BEGIN
    SELECT sp.id_scrapProducto,
           sp.fecha,
           per.nombre AS Usuario,
           prod.nombre AS Producto,
           /* Nota: MotivoScrap aquí no tiene columna 'descripcion' en la estructura original.
              Si antes usabas una columna descripcion en MotivoScrap, adaptala.
              Actualmente MotivoScrap tiene flags (dano, vencido, ...) */
           msc.id_motivoScrap AS MotivoId,
           sp.cantidad
    FROM ScrapProducto sp
    INNER JOIN Usuarios u ON sp.id_usuario = u.id_usuario
    INNER JOIN personas per ON u.id_persona = per.id_persona
    INNER JOIN Productos prod ON sp.id_producto = prod.id_producto
    INNER JOIN MotivoScrap msc ON sp.id_motivoScrap = msc.id_motivoScrap
    WHERE sp.fecha BETWEEN @fechaDesde AND @fechaHasta
END
GO

--clientes 
DROP PROCEDURE IF EXISTS sp_AgregarCliente;
GO
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

DROP PROCEDURE IF EXISTS sp_ModificarCliente;
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

DROP PROCEDURE IF EXISTS sp_EliminarCliente;
GO
CREATE PROCEDURE sp_EliminarCliente
    @id_cliente INT
AS
BEGIN
    DELETE FROM Clientes WHERE id_cliente = @id_cliente
END
GO

DROP PROCEDURE IF EXISTS sp_ConsultarClientePorNombre;
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

DROP PROCEDURE IF EXISTS sp_ConsultarClientePorCUIT;
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

DROP PROCEDURE IF EXISTS sp_AgregarTelefonoCliente;
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

DROP PROCEDURE IF EXISTS sp_AgregarDireccionCliente;
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
DROP PROCEDURE IF EXISTS sp_RegistrarPresupuestoCompra;
GO
CREATE PROCEDURE sp_RegistrarPresupuestoCompra
    @id_proveedor INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO Compras (id_proveedor, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoCompras)
    VALUES (@id_proveedor, @fecha, 'Presupuesto', NULL, @total, NULL)
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarOrdenCompra;
GO
CREATE PROCEDURE sp_RegistrarOrdenCompra
    @id_presupuesto INT,
    @fecha DATE,
    @total DECIMAL(18,2)
AS
BEGIN
    INSERT INTO Compras (id_proveedor, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoCompras)
    VALUES (@id_presupuesto, @fecha, 'OrdenCompra', NULL, @total, NULL)
END
GO

DROP PROCEDURE IF EXISTS sp_ConsultarPedidos;
GO
CREATE PROCEDURE sp_ConsultarPedidos
    @entregado BIT = NULL
AS
BEGIN
    SELECT 
        oc.id_compra AS id_ordenCompra,
        p.razonSocial AS proveedor,
        oc.fecha,
        oc.montoTotal AS total,
        /* campo 'entregado' no existe en Compras original -> si lo necesitás, agregalo a la tabla Compras */
        NULL AS entregado
    FROM Compras oc
    INNER JOIN Proveedores p ON oc.id_proveedor = p.id_proveedor
    WHERE (@entregado IS NULL OR NULL = @entregado)
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarRemito;
GO
CREATE PROCEDURE sp_RegistrarRemito
    @id_ordenCompra INT,
    @numeroRemito VARCHAR(50),
    @fecha DATE,
    @conFactura BIT
AS
BEGIN
    IF OBJECT_ID('dbo.Remito','U') IS NULL
    BEGIN
        CREATE TABLE Remito (
            id_remito INT IDENTITY(1,1) PRIMARY KEY,
            id_ordenCompra INT,
            numeroRemito VARCHAR(50),
            fecha DATE,
            conFactura BIT,
            CONSTRAINT FK_Remito_OrdenCompra FOREIGN KEY (id_ordenCompra) REFERENCES Compras(id_compra)
        );
    END

    INSERT INTO Remito (id_ordenCompra, numeroRemito, fecha, conFactura)
    VALUES (@id_ordenCompra, @numeroRemito, @fecha, @conFactura)
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarFacturaCompra;
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
    INSERT INTO Compras (id_proveedor, fecha, tipoDocumento, numeroDocumento, montoTotal, id_estadoCompras)
    VALUES (@id_proveedor, @fecha, 'FacturaCompra', @numeroFactura, @total, NULL)
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarNotaCredito;
GO
CREATE PROCEDURE sp_RegistrarNotaCredito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    IF OBJECT_ID('dbo.NotaCredito','U') IS NULL
    BEGIN
        CREATE TABLE NotaCredito (
            id_notaCredito INT IDENTITY(1,1) PRIMARY KEY,
            id_factura INT,
            fecha DATE,
            monto DECIMAL(18,2),
            motivo VARCHAR(255),
            CONSTRAINT FK_NotaCredito_Compras FOREIGN KEY (id_factura) REFERENCES Compras(id_compra)
        );
    END

    INSERT INTO NotaCredito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

DROP PROCEDURE IF EXISTS sp_RegistrarNotaDebito;
GO
CREATE PROCEDURE sp_RegistrarNotaDebito
    @id_factura INT,
    @fecha DATE,
    @monto DECIMAL(18,2),
    @motivo VARCHAR(255)
AS
BEGIN
    IF OBJECT_ID('dbo.NotaDebito','U') IS NULL
    BEGIN
        CREATE TABLE NotaDebito (
            id_notaDebito INT IDENTITY(1,1) PRIMARY KEY,
            id_factura INT,
            fecha DATE,
            monto DECIMAL(18,2),
            motivo VARCHAR(255),
            CONSTRAINT FK_NotaDebito_Compras FOREIGN KEY (id_factura) REFERENCES Compras(id_compra)
        );
    END

    INSERT INTO NotaDebito (id_factura, fecha, monto, motivo)
    VALUES (@id_factura, @fecha, @monto, @motivo)
END
GO

DROP PROCEDURE IF EXISTS sp_ReporteCompras;
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

DROP PROCEDURE IF EXISTS sp_ReporteDevolucionesProveedores;
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
    FROM DevolucionesProveedor d
    INNER JOIN Proveedores p ON d.id_devolucion IS NOT NULL /* placeholder join; no direct FK to proveedor in this table */
    INNER JOIN Productos pr ON d.id_devolucion IS NOT NULL /* placeholder: ajustá según tu lógica real */
    WHERE 
        (@fechaInicio IS NULL OR d.fecha >= @fechaInicio) AND
        (@fechaFin IS NULL OR d.fecha <= @fechaFin)
END
GO

--productos
DROP PROCEDURE IF EXISTS sp_AgregarProducto;
GO
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

DROP PROCEDURE IF EXISTS sp_ModificarProducto;
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

DROP PROCEDURE IF EXISTS sp_EliminarProducto;
GO
CREATE PROCEDURE sp_EliminarProducto
    @id_producto INT
AS
BEGIN
    DELETE FROM Productos WHERE id_producto = @id_producto
END
GO

DROP PROCEDURE IF EXISTS sp_ObtenerProductos;
GO
CREATE PROCEDURE sp_ObtenerProductos
AS
BEGIN
    SELECT p.id_producto, p.codigo, p.nombre, p.descripcion, m.marca AS Marca, c.categoria AS Categoria,
           p.precioCompra, p.precioVenta, p.estado, p.ubicacion, p.habilitado
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
END
GO

DROP PROCEDURE IF EXISTS sp_BuscarProducto;
GO
CREATE PROCEDURE sp_BuscarProducto
    @busqueda VARCHAR(100)
AS
BEGIN
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @busqueda + '%'
       OR p.codigo LIKE '%' + @busqueda + '%'
       OR m.marca LIKE '%' + @busqueda + '%'
       OR c.categoria LIKE '%' + @busqueda + '%'
END
GO

DROP PROCEDURE IF EXISTS sp_ProductosHabilitados;
GO
CREATE PROCEDURE sp_ProductosHabilitados
AS
BEGIN
    SELECT * FROM Productos WHERE habilitado = 1
END
GO

-- Continuación: proveedores y auxiliares
DROP PROCEDURE IF EXISTS sp_ProductosPorCategoria;
DROP PROCEDURE IF EXISTS sp_ProductosPorMarca;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProductoPorCodigo;
DROP PROCEDURE IF EXISTS sp_AgregarProveedor;
DROP PROCEDURE IF EXISTS sp_ModificarProveedor;
DROP PROCEDURE IF EXISTS sp_EliminarProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorNombre;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorCUIT;
DROP PROCEDURE IF EXISTS sp_AgregarTelefonoProveedor;
DROP PROCEDURE IF EXISTS sp_AgregarDireccionProveedor;
DROP PROCEDURE IF EXISTS sp_RelacionarProductoProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProductosPorProveedor;
DROP PROCEDURE IF EXISTS sp_ConsultarProveedoresPorProducto;
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
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.nombre LIKE '%' + @nombre + '%'
END
GO

CREATE PROCEDURE sp_ConsultarProductoPorCodigo
    @codigo VARCHAR(50)
AS
BEGIN
    SELECT p.*, m.marca AS Marca, c.categoria AS Categoria
    FROM Productos p
    LEFT JOIN MarcasProducto m ON p.id_marca = m.id_marca
    LEFT JOIN CategoriasProducto c ON p.id_categoria = c.id_categoria
    WHERE p.codigo = @codigo
END
GO

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
    INSERT INTO Proveedores (codigo, nombre, razonSocial, CUIT, TiempoEntrega, Descuento, id_formaPago)
    VALUES (@codigo, @razonSocial, @razonSocial, @cuit, NULL, @descuento, NULL)
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
    UPDATE Proveedores
    SET codigo = @codigo,
        nombre = @razonSocial,
        razonSocial = @razonSocial,
        CUIT = @cuit,
        TiempoEntrega = TRY_CAST(@tiempoEntrega AS INT),
        Descuento = @descuento
    WHERE id_proveedor = @id_proveedor
END
GO

CREATE PROCEDURE sp_EliminarProveedor
    @id_proveedor INT
AS
BEGIN
    DELETE FROM Proveedores WHERE id_proveedor = @id_proveedor
END
GO

CREATE PROCEDURE sp_ConsultarProveedoresPorNombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT 
        id_proveedor,
        codigo,
        nombre,
        razonSocial,
        CUIT,
        TiempoEntrega,
        Descuento
    FROM Proveedores
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
        nombre,
        razonSocial,
        CUIT,
        TiempoEntrega,
        Descuento
    FROM Proveedores
    WHERE CUIT LIKE '%' + @cuit + '%'
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
    INSERT INTO ProveedorTelefonos (id_proveedor, telefono, sector, horario, email)
    VALUES (@id_proveedor, @telefono, @sector, @horario, @email)
END
GO

CREATE PROCEDURE sp_AgregarDireccionProveedor
    @id_proveedor INT,
    @direccion VARCHAR(255),
    @localidad VARCHAR(100),
    @provincia VARCHAR(100)
AS
BEGIN
    INSERT INTO ProveedorUbicacion (id_proveedor, direccion, localidad, provincia)
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
        p.precioVenta AS precio
    FROM ProductoProveedor pp
    INNER JOIN Productos p ON pp.id_producto = p.id_producto
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
        pr.CUIT,
        pr.id_formaPago,
        pr.Descuento
    FROM ProductoProveedor pp
    INNER JOIN Proveedores pr ON pp.id_proveedor = pr.id_proveedor
    WHERE pp.id_producto = @id_producto
END
GO

-- TABLAS AUXILIARES: Categorías, Marcas, Formas de Pago, Estados

DROP PROCEDURE IF EXISTS sp_AgregarCategoria;
DROP PROCEDURE IF EXISTS sp_ModificarCategoria;
DROP PROCEDURE IF EXISTS sp_EliminarCategoria;
DROP PROCEDURE IF EXISTS sp_AgregarMarca;
DROP PROCEDURE IF EXISTS sp_ModificarMarca;
DROP PROCEDURE IF EXISTS sp_EliminarMarca;
DROP PROCEDURE IF EXISTS sp_AgregarFormaPago;
DROP PROCEDURE IF EXISTS sp_ModificarFormaPago;
DROP PROCEDURE IF EXISTS sp_EliminarFormaPago;
GO

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
DROP PROCEDURE IF EXISTS sp_ModificarEstadoCompra;
GO
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
DROP PROCEDURE IF EXISTS sp_ModificarEstadoVenta;
GO
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
DROP PROCEDURE IF EXISTS sp_AgregarPresupuestoVenta;
DROP PROCEDURE IF EXISTS sp_AgregarNotaPedido;
DROP PROCEDURE IF EXISTS sp_AgregarNotaCredito;
DROP PROCEDURE IF EXISTS sp_AgregarNotaDebito;
DROP PROCEDURE IF EXISTS sp_DevolverProductoStock;
DROP PROCEDURE IF EXISTS sp_ReporteVentas;
GO

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