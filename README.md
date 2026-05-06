# Projeto Banco — API

## 1. Aluno e RMs

| Nome completo | RM |
|---|---|
| Isabelle Dallabeneta Carlesso | RM554592 |
| Nicoli Amy Kassa | RM559104 |


---

## 2. Produto bancário escolhido

**Produto:** Empréstimo (`Emprestimo`)

**Justificativa:** O produto de empréstimo foi escolhido por permitir a implementação de uma regra de negócio clara e objetiva: a análise e aprovação do crédito com base no valor solicitado pelo cliente. Isso viabiliza a demonstração completa do fluxo de contratação — desde o cadastro do cliente até a aprovação ou recusa — de forma síncrona e rastreável, sem depender de integrações externas complexas.

---

## 4. Diagrama de classes

> Insira aqui a imagem exportada do draw.io:

![Diagrama de Classes](./docs/diagrama-classes.png)

**Estrutura do domínio:**

- `Cliente` é uma classe abstrata com discriminator (`Tipo`) que diferencia `PessoaFisica` e `PessoaJuridica`
- `Produto` é uma classe abstrata com três especializações: `Emprestimo`, `MaquinaDeCartao` e `ReceberSalario`
- Um cliente pertence a uma única `Agencia` (`N → 1`)
- Um cliente pode ter múltiplas `Contratacao` ao longo do tempo (`1 → N`)

---

## 5. Como rodar localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- Acesso à rede da FIAP (VPN ou rede interna) para o banco Oracle
- Credenciais Oracle individuais (RM + senha)

### Configurar a connection string

Edite o arquivo `appsettings.json` na raiz do projeto:

```json
"ConnectionStrings": {
    "OracleConnection": "User Id=RMXXXXXX;Password=DDMMAA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
```

### Aplicar as migrations

```bash
dotnet ef database update
```

### Rodar a API

```bash
dotnet run
```

A API estará disponível em `https://localhost:{porta}/swagger`.

---

## 6. Endpoints disponíveis

### POST `/api/agencias` — Cadastrar agência

**Request:**
```json
{
  "nome": "Agência Central",
  "cidade": "São Paulo"
}
```

**Response `201 Created`:**
```json
{
  "id": 1,
  "nome": "Agência Central",
  "cidade": "São Paulo"
}
```

---

### GET `/api/agencias/{id}` — Buscar agência

**Response `200 OK`:**
```json
{
  "id": 1,
  "nome": "Agência Central",
  "cidade": "São Paulo"
}
```

**Response `404 Not Found`:**
```json
"Agência não encontrada."
```

---

### POST `/api/clientes/pf` — Cadastrar pessoa física

**Request:**
```json
{
  "nome": "João Silva",
  "cpf": "123.456.789-00",
  "dataNascimento": "1990-05-15",
  "agenciaId": 1
}
```

**Response `201 Created`:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "cpf": "123.456.789-00",
  "dataNascimento": "1990-05-15T00:00:00",
  "agenciaId": 1
}
```

**Response `400 Bad Request` (CPF duplicado):**
```json
"CPF já cadastrado."
```

**Response `404 Not Found` (agência inexistente):**
```json
"Agência não encontrada."
```

---

### POST `/api/clientes/pj` — Cadastrar pessoa jurídica

**Request:**
```json
{
  "nome": "Empresa XYZ",
  "cnpj": "12.345.678/0001-99",
  "razaoSocial": "Empresa XYZ Ltda",
  "agenciaId": 1
}
```

**Response `201 Created`:**
```json
{
  "id": 2,
  "nome": "Empresa XYZ",
  "cnpj": "12.345.678/0001-99",
  "razaoSocial": "Empresa XYZ Ltda",
  "agenciaId": 1
}
```

**Response `400 Bad Request` (CNPJ duplicado):**
```json
"CNPJ já cadastrado."
```

---

### GET `/api/clientes/{id}` — Buscar cliente por ID

**Response `200 OK`:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "cpf": "123.456.789-00",
  "dataNascimento": "1990-05-15T00:00:00",
  "agenciaId": 1
}
```

**Response `404 Not Found`:**
```json
"Cliente não encontrado."
```

---

### POST `/api/contratacoes` — Solicitar contratação

**Request:**
```json
{
  "clienteId": 1,
  "produto": "Emprestimo",
  "valorSolicitado": 5000.00
}
```

**Response `202 Accepted`:**
```json
{
  "id": 1,
  "clienteId": 1,
  "produto": "Emprestimo",
  "status": "Aprovado",
  "solicitadoEm": "2026-05-06T14:00:00Z"
}
```

**Response `404 Not Found` (cliente inexistente):**
```json
"Cliente não encontrado."
```

**Regra de negócio:** Empréstimos com `valorSolicitado` acima de R$ 10.000,00 são automaticamente recusados. Valores iguais ou inferiores são aprovados.

---

### GET `/api/contratacoes/{id}` — Consultar status da contratação

**Response `200 OK`:**
```json
{
  "id": 1,
  "clienteId": 1,
  "produto": "Emprestimo",
  "status": "Aprovado",
  "solicitadoEm": "2026-05-06T14:00:00Z"
}
```

**Response `404 Not Found`:**
```json
"Contratação não encontrada."
```

---

## 7. POSTs e GETs

#### POST agencias

![POST agencias](./CP2-CSharp/docs/img/agenciasPOST.png)


---

## 8. Testes de erros

#### Cadastro de PF com CPF duplicado → `400`

![Print dos testes](./docs/print-testes.png)


#### Cadastro de PJ com CNPJ duplicado → `400`

![Print dos testes](./docs/print-testes.png)


#### Vincular cliente a agência inexistente → `404`

![Print dos testes](./docs/print-testes.png)


#### Contratação válida → `202` com status `Aprovado`

![Print dos testes](./docs/print-testes.png)


#### Contratação para cliente inexistente → `404`

![Print dos testes](./docs/print-testes.png)


#### Consulta de status após processamento → `200`

![Print dos testes](./docs/print-testes.png)

---

## 9. API no Swagger  

![Swagger contratação aprovada](./docs/print-swagger.png)