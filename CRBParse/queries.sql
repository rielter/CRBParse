CREATE TABLE [dbo].[Curs] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [CursDate]    DATETIME       NOT NULL,
    [EuroValue]   DECIMAL (9, 4) NOT NULL,
    [DollarValue] DECIMAL (9, 4) NOT NULL,
    [IsApply]     BIT            NOT NULL
);

GO
ALTER TABLE [dbo].[Curs]
    ADD CONSTRAINT [PK_dbo.Curs] PRIMARY KEY CLUSTERED ([ID] ASC);		

GO
CREATE NONCLUSTERED INDEX [CursDate]
    ON [dbo].[Curs]([CursDate] ASC);
	
GO
CREATE PROCEDURE [dbo].GetCurses
AS
	SELECT * FROM Curs ORDER BY CursDate DESC
	
GO
CREATE PROCEDURE [dbo].SetCursApply
AS
	UPDATE Curs
	SET [IsApply] = 'True'
	WHERE [CursDate] = (SELECT MAX([CursDate]) FROM Curs)

GO	
CREATE PROCEDURE [dbo].[AddCurs]
	@cursDate datetime,
	@euroValue decimal(9,4),
	@dollarValue decimal(9,4)
AS
	DECLARE @currentDollar decimal(9,4), @isApply bit;
	SET @currentDollar = (SELECT TOP 1 [DollarValue] FROM Curs WHERE [CursDate] = (SELECT MAX([CursDate]) FROM Curs));
	SET @isApply = 1;

	IF (@dollarValue < @currentDollar)
		BEGIN
			SET @dollarValue = @currentDollar;
			SET @euroValue = (SELECT TOP 1 [EuroValue] FROM Curs WHERE [CursDate] = (SELECT MAX([CursDate]) FROM Curs));
			SET @isApply = 0;
		END

	INSERT INTO [Curs] ([CursDate], [EuroValue], [DollarValue], [IsApply]) VALUES (@cursDate, @euroValue, @dollarValue, @isApply);
