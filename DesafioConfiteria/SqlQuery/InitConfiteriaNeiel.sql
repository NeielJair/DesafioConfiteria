use ConfiteriaNeiel
GO

--- Tablas
-- Local
CREATE TABLE Local(
	IdLocal	int primary key identity(1,1),
	FechaBaja datetime,
	Nombre varchar(max) NOT NULL,
	Direccion varchar(max) NOT NULL,
	Telefono varchar(max) NOT NULL,
	Email varchar(max) NOT NULL
);
GO

-- Mozo
CREATE TABLE Mozo(
	IdMozo int primary key identity(1,1),
	IdLocal int foreign key references Local(IdLocal) NOT NULL,
	FechaBaja datetime,
	FechaContrato datetime,
	Nombre varchar(max) NOT NULL,
	Apellido varchar(max) NOT NULL,
	Documento int NOT NULL,
	Comision int NOT NULL
);
GO

-- Rubro
CREATE TABLE Rubro(
	IdRubro int primary key identity(1,1),
	IdLocal int foreign key references Local(IdLocal) NOT NULL,
	FechaBaja datetime,
	Nombre varchar(max) NOT NULL
);
GO

-- Articulo
CREATE TABLE Articulo(
	IdArticulo int primary key identity(1,1),
	IdLocal int foreign key references Local(IdLocal) NOT NULL,
	FechaBaja datetime,
	Nombre varchar(max) NOT NULL,
	IdRubro int foreign key references Rubro(IdRubro)
);
GO

-- Ticket
CREATE TABLE Ticket(
	IdTicket int primary key identity(1,1),
	IdLocal int foreign key references Local(IdLocal) NOT NULL,
	FechaBaja datetime,
	FechaVenta datetime NOT NULL,
	IdMozo int foreign key references Mozo(IdMozo)
);
GO

-- DetalleTicket
CREATE TABLE DetalleTicket(
	IdDetalle int primary key identity(1,1),
	IdTicket int foreign key references Ticket(IdTicket) NOT NULL,
	IdArticulo int foreign key references Articulo(IdArticulo) NOT NULL,
	Cantidad int NOT NULL
);
GO


--- Stored procedures
-- Local
CREATE PROCEDURE BuscarLocales
AS
	SELECT IdLocal, FechaBaja, Nombre, Direccion, Telefono, Email
	FROM Local;
GO

CREATE PROCEDURE BuscarLocalesActivos
AS
	SELECT IdLocal, Nombre, Direccion, Telefono, Email
	FROM Local
	WHERE FechaBaja IS NULL;
GO

CREATE PROCEDURE BuscarLocalPorId
	@IdLocal int
AS
	SELECT FechaBaja, Nombre, Direccion, Telefono, Email
	FROM Local
	WHERE IdLocal = @IdLocal;
GO

-- Mozo
CREATE PROCEDURE BuscarMozoPorId
	@IdMozo int
AS
	SELECT IdLocal, FechaBaja, FechaContrato, Nombre, Apellido, Documento, Comision
	FROM Mozo
	WHERE IdMozo = @IdMozo;
GO

CREATE PROCEDURE BuscarMozosPorIdLocal
	@IdLocal int
AS
	SELECT IdMozo, FechaBaja, FechaContrato, Nombre, Apellido, Documento, Comision
	FROM Mozo
	WHERE IdLocal = @IdLocal;
GO

CREATE PROCEDURE BuscarMozosActivosPorIdLocal
	@IdLocal int
AS
	SELECT IdMozo, FechaContrato, Nombre, Apellido, Documento, Comision
	FROM Mozo
	WHERE IdLocal = @IdLocal AND FechaBaja IS NULL;
GO

-- Rubro
CREATE PROCEDURE BuscarRubroPorId
	@IdRubro int
AS
	SELECT IdLocal, FechaBaja, Nombre
	FROM Rubro
	WHERE IdRubro = @IdRubro;
GO

CREATE PROCEDURE BuscarRubrosPorIdLocal
	@IdLocal int
AS
	SELECT IdRubro, FechaBaja, Nombre
	FROM Rubro
	WHERE IdLocal = @IdLocal;
GO

CREATE PROCEDURE BuscarRubrosActivosPorIdLocal
	@IdLocal int
AS
	SELECT IdRubro, Nombre
	FROM Rubro
	WHERE IdLocal = @IdLocal AND FechaBaja IS NULL;
GO

-- Articulo
CREATE PROCEDURE BuscarArticuloPorId
	@IdArticulo int
AS
	SELECT IdLocal, FechaBaja, Nombre
	FROM Articulo
	WHERE IdArticulo = @IdArticulo;
GO

CREATE PROCEDURE BuscarArticulosPorIdLocal
	@IdLocal int
AS
	SELECT IdArticulo, FechaBaja, Nombre, IdRubro
	FROM Articulo
	WHERE IdLocal = @IdLocal;
GO

CREATE PROCEDURE BuscarArticulosActivosPorIdLocal
	@IdLocal int
AS
	SELECT IdArticulo, FechaBaja, Nombre, IdRubro
	FROM Articulo
	WHERE IdLocal = @IdLocal AND FechaBaja IS NULL;
GO

-- Ticket
CREATE PROCEDURE BuscarTicketPorId
	@IdTicket int
AS
	SELECT IdLocal, FechaBaja, FechaVenta, IdMozo
	FROM Ticket
	WHERE IdTicket = IdTicket;
GO

CREATE PROCEDURE BuscarTicketsPorLocalId
	@IdLocal int
AS
	SELECT IdTicket, FechaBaja, FechaVenta, IdMozo
	FROM Ticket
	WHERE IdLocal = @IdLocal;
GO

CREATE PROCEDURE BuscarTicketsActivosPorLocalId
	@IdLocal int
AS
	SELECT IdTicket, FechaBaja, FechaVenta, IdMozo
	FROM Ticket
	WHERE IdLocal = @IdLocal AND FechaBaja IS NULL;
GO

-- DetalleTicket
CREATE PROCEDURE BuscarDetalleTicketPorId
	@IdDetalle int
AS
	SELECT IdTicket, IdArticulo, Cantidad
	FROM DetalleTicket
	WHERE IdDetalle = @IdDetalle;
GO

CREATE PROCEDURE BuscarDetallesTicketPorTicketId
	@IdTicket int
AS
	SELECT IdDetalle, IdArticulo, Cantidad
	From DetalleTicket
	WHERE IdTicket = @IdTicket;
GO