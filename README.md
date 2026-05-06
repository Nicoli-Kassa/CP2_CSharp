# Projeto Banco — API

## 1. Alunos e RMs

| Nome completo | RM |
|---|---|
| Isabelle Dallabeneta Carlesso | RM554592 |
| Nicoli Amy Kassa | RM559104 |

---

## 2. Produto bancário escolhido

**Produto:** Empréstimo (`Emprestimo`)

**Justificativa:** O produto de empréstimo foi escolhido por permitir a implementação de uma regra de negócio clara e objetiva: a análise e aprovação do crédito com base no valor solicitado, no valor máximo permitido, na taxa de juros e no prazo em meses. Isso viabiliza a demonstração completa do fluxo de contratação — desde o cadastro do cliente até a aprovação ou recusa — de forma assíncrona e rastreável.

---

## 3. Diagrama de classes

![Diagrama de Classes](./CP2-CSharp/docs/img/diagrama-classe.png)

---

## 4. Como rodar localmente

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

## 5. Endpoints disponíveis

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

**Response `400 Bad Request` (nome vazio):**
```json
"Nome da agência é obrigatório."
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
  "cpf": "123.456.789-00",
  "dataNascimento": "1990-05-15T00:00:00",
  "id": 1,
  "nome": "João Silva",
  "agenciaId": 1,
  "agencia": {
    "id": 1,
    "nome": "Agência Central",
    "cidade": "São Paulo"
  }
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
  "cnpj": "12.345.678/0001-99",
  "razaoSocial": "Empresa XYZ Ltda",
  "id": 4,
  "nome": "Empresa XYZ",
  "agenciaId": 1,
  "agencia": {
    "id": 1,
    "nome": "Agência Central",
    "cidade": "São Paulo"
  }
}
```

**Response `400 Bad Request` (CNPJ duplicado):**
```json
"CNPJ já cadastrado."
```

**Response `404 Not Found` (agência inexistente):**
```json
"Agência não encontrada."
```

---

### GET `/api/clientes/{id}` — Buscar cliente por ID

**Response `200 OK` (Pessoa Física):**
```json
{
  "cpf": "123.456.789-00",
  "dataNascimento": "1990-05-15T00:00:00",
  "id": 1,
  "nome": "João Silva",
  "agenciaId": 1
}
```

**Response `404 Not Found`:**
```json
"Cliente não encontrado."
```

---

### POST `/api/emprestimos` — Cadastrar produto de empréstimo

**Request:**
```json
{
  "nome": "Empréstimo Pessoal",
  "descricao": "Crédito para uso pessoal",
  "valorMaximo": 10000.00,
  "taxaJuros": 5.5,
  "prazoMeses": 24
}
```

**Response `201 Created`:**
```json
{
  "id": 1,
  "nome": "Empréstimo Pessoal",
  "descricao": "Crédito para uso pessoal",
  "valorMaximo": 10000.00,
  "taxaJuros": 5.5,
  "prazoMeses": 24
}
```

**Response `400 Bad Request` (valor máximo inválido):**
```json
"Valor máximo deve ser maior que zero."
```

**Response `400 Bad Request` (taxa de juros inválida):**
```json
"Taxa de juros inválida."
```

**Response `400 Bad Request` (prazo inválido):**
```json
"Prazo em meses deve ser maior que zero."
```

---

### GET `/api/emprestimos/{id}` — Buscar empréstimo por ID

**Response `200 OK`:**
```json
{
  "id": 1,
  "nome": "Empréstimo Pessoal",
  "descricao": "Crédito para uso pessoal",
  "valorMaximo": 10000.00,
  "taxaJuros": 5.5,
  "prazoMeses": 24
}
```

**Response `404 Not Found`:**
```json
"Empréstimo não encontrado."
```

---

### POST `/api/contratacoes` — Solicitar contratação

**Request:**
```json
{
  "clienteId": 1,
  "produtoId": 1,
  "valorSolicitado": 5000.00
}
```

**Response `202 Accepted`:**
```json
{
  "mensagem": "Contratação recebida e em processamento",
  "id": 1,
  "status": "PENDENTE"
}
```

**Response `404 Not Found` (cliente inexistente):**
```json
"Cliente não encontrado."
```

**Response `400 Bad Request` (valor acima do limite do produto):**
```json
"Valor solicitado acima do limite permitido para este empréstimo."
```

**Response `500 Internal Server Error` (falha ao salvar no banco):**
```json
"Erro ao salvar contratação."
```

**Regra de negócio:** O empréstimo é avaliado com base em três critérios: o valor solicitado não pode ultrapassar o `valorMaximo` do produto; a taxa de juros é aplicada sobre o prazo em meses para calcular o total a pagar; taxas acima de 10% geram aprovação com ressalva. Valores acima do limite são automaticamente reprovados.

---

### GET `/api/contratacoes/{id}` — Consultar status da contratação

**Response `200 OK`:**
```json
{
  "id": 1,
  "clienteId": 1,
  "produtoId": 1,
  "valorSolicitado": 5000.00,
  "status": "APROVADO",
  "solicitadoEm": "2026-05-06T14:00:00Z"
}
```

**Response `404 Not Found`:**
```json
"Contratação não encontrada."
```

---

## 6. POSTs e GETs

#### POST agencias

![POST agencias](./CP2-CSharp/docs/img/agenciasPOST.png)

#### GET agencias

![GET agencias](./CP2-CSharp/docs/img/agenciasGET.png)

#### POST clientes PF

![POST clientes PF](./CP2-CSharp/docs/img/clientePF.png)

#### POST clientes PJ

![POST clientes PJ](./CP2-CSharp/docs/img/clientePJ.png)

#### GET clientes por ID

![GET clientes por ID](./CP2-CSharp/docs/img/clienteGET.png)

#### POST emprestimos

![POST emprestimos](./CP2-CSharp/docs/img/emprestimoPOST.png)

#### POST contratacoes

![POST contratacoes](./CP2-CSharp/docs/img/contratacaoPOST.png)

#### GET contratacoes

![GET contratacoes](./CP2-CSharp/docs/img/contratacaoGET.png)

#### GET ID contratacoes

![GET ID contratacoes](./CP2-CSharp/docs/img/contratacaoGETid.png)

---

## 7. Testes de erros

#### Cadastro de agência com nome vazio → `400`

![Nome da agência vazio](./CP2-CSharp/docs/img/agenciasNomeVazio.png)

#### Cadastro de PF com CPF duplicado → `400`

![CPF duplicado](./CP2-CSharp/docs/img/clienteCPFduplicado.png) 

#### Cadastro de PJ com CNPJ duplicado → `400`

![CNPJ duplicado](./CP2-CSharp/docs/img/clienteCNPJduplicado.png)

#### Vincular cliente a agência inexistente → `404`

![Agência não encontrada](./CP2-CSharp/docs/img/clienteAgenciaNaoEncontrada.png)

#### Cadastro de empréstimo com taxa de juros inválida → `400`

![Taxa de juros inválida](./CP2-CSharp/docs/img/emprestimoTaxaJurosInva.png)

#### Cadastro de empréstimo com prazo inválido → `400`

![Prazo inválido](./CP2-CSharp/docs/img/emprestimoPrazoInva.png)

#### Contratação com valor acima do limite do produto → `400`

![Valor acima do limite](./CP2-CSharp/docs/img/emprestimoValorMaxInva.png)

#### Contratação para cliente inexistente → `404`

![Cliente não encontrado](./CP2-CSharp/docs/img/contratacaoClienteNaoEncontrado.png)

#### Consulta de status após processamento → `200`

![Contratação pendente](./CP2-CSharp/docs/img/contratacaoPendente.png)
![Contratação aprovada](./CP2-CSharp/docs/img/contratacaoAprovada.png)

---

## 8. API no Swagger

![API no Swagger](./CP2-CSharp/docs/img/APISwagger.png)