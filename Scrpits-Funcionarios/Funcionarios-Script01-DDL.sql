Create DATABASE T_Peoples
USE T_Peoples;

CREATE TABLE Funcionarios (
IdFuncionario INT PRIMARY KEY IDENTITY,
Nome varchar (250),
Sobrenome varchar (250)
);

alter table Funcionarios
add DataNascimento datetime






