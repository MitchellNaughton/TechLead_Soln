CREATE TABLE [dbo].[tmTax]
(
	[iUid] INT NOT NULL PRIMARY KEY IDENTITY, 
    [iCountryId] INT NULL, 
    [flTaxInc1] FLOAT NULL, 
    [flTaxInc2] FLOAT NULL, 
    [flTaxInc3] FLOAT NULL, 
    [flTaxRate1] FLOAT NULL, 
    [flTaxRate2] FLOAT NULL, 
    [flTaxRate3] FLOAT NULL, 
    CONSTRAINT [FK_tmTax_tmCountry] FOREIGN KEY ([iCountryId]) REFERENCES [tmCountry]([iUid])
)
