--**************************SimulacaoCalculoJuros**************************--

CREATE TABLE Parcela (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    NumParc INT NOT NULL,
    Valor DECIMAL(20,2) NOT NULL,
    DataVencimento DATETIME NOT NULL,
    DataInclusao DATETIME NOT NULL
);

CREATE TABLE Juros (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    IdParcela INT NOT NULL,
    IdSimulacao INT NOT NULL,
    ValorJuros DECIMAL(20,2) NOT NULL,
);

CREATE TABLE Simulacao (
    Id INT PRIMARY KEY IDENTITY (1, 1),
    PorcentagemJuros DECIMAL(6,3) NOT NULL,
    TipoCalculo INT NOT NULL,
    TotalJuros DECIMAL(20,2) NOT NULL,
    TotalDivida DECIMAL(20,2) NOT NULL,
    DataInclusao DATETIME NOT NULL
);
