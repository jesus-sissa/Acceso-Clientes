use SIAC
go
CREATE PROCEDURE web_Sevicios_Cheques
(
	@Id_Ficha int
)
AS
BEGIN
	SELECT	b.Nombre		As	[Banco],
			fc.Cuenta		As	[Cuenta],
			fc.Importe		As	[Importe],
			fc.Numero		As	[Numero]
	FROM	pro_FichasCheques	fc
	JOIN	Cat_Bancos			b	on	b.Id_Banco	=	fc.Id_Banco
	WHERE	Id_Ficha	=	@Id_Ficha
END