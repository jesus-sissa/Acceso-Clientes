USE [SIAC]
GO

/****** Object:  Table [dbo].[Cli_Incidencias]    Script Date: 11/20/2010 12:52:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Cli_Incidencias](
	[Id_Incidencia] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Id_Usuario] [numeric](18, 0) NOT NULL,
	[Numero_Incidencia] [numeric](18, 0) NOT NULL,
	[Tipo] [tinyint] NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
	[Descripcion] [text] NOT NULL,
	[Fecha_Registro] [datetime] NOT NULL,
	[Estacion_Registro] [varchar](50) NOT NULL,
	[EstacionIP_Registro] [varchar](50) NOT NULL,
	[Fecha_Solucion] [datetime] NULL,
	[Usuario_Solucion] [numeric](18, 0) NULL,
	[Solucion] [text] NULL,
	[Status] [varchar](1) NOT NULL,
 CONSTRAINT [PK_Cli_Incidencia] PRIMARY KEY CLUSTERED 
(
	[Id_Incidencia] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

