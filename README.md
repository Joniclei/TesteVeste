# Desafio Técnico — Desenvolvedor C# / .NET

## Contexto

Você fará parte de um time que desenvolve APIs RESTful em **C# com .NET 8**, seguindo Clean Architecture.

Este projeto simula um pequeno sistema de gestão de produtos de uma loja de roupas.
A estrutura já está criada e parte do código já está implementado como referência.  
**Sua tarefa é completar a implementação do CRUD de Produtos.**

---

## Estrutura do Projeto

```
TesteVeste/
├── src/
│   ├── TesteVeste.Domain/          → Entidades e interfaces de repositório
│   ├── TesteVeste.Application/     → Serviços, DTOs e interfaces de serviço
│   ├── TesteVeste.Infrastructure/  → EF Core InMemory, repositórios concretos
│   └── TesteVeste.API/             → Controllers ASP.NET Core + Scalar
└── tests/
    └── TesteVeste.Tests/           → Testes unitários (xUnit + Moq)
```

> **Arquivos de referência** — estão completamente implementados para você consultar:
> - `CategoriaService.cs` → veja como implementar `ProdutoService.cs`
> - `CategoriaRepository.cs` → veja como implementar `ProdutoRepository.cs`
> - `CategoriasController.cs` → veja como implementar `ProdutosController.cs`

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Git

---

## Como rodar

```bash
# Clonar o projeto
git clone https://github.com/Joniclei/TesteVeste.git
cd TesteVeste

# Rodar a API (sem precisar de banco de dados)
dotnet run --project src/TesteVeste.API

# Acessar a documentação da API (Scalar)
# http://localhost:5285/scalar/v1
```

> **Autenticação:** todos os endpoints exigem Bearer token JWT.
> Obtenha o token primeiro:
> ```
> POST /api/auth/token
> { "usuario": "admin", "senha": "123456" }
> ```
> No Scalar, clique em **Authentication → Bearer** e cole o token retornado.

---

## O que você deve implementar

### 1. `ProdutoService.cs` — Application/Services

Implemente os métodos do serviço respeitando as **regras de negócio**:

| # | Regra |
|---|-------|
| 1 | Nome é **obrigatório** e deve ter no máximo **100 caracteres** |
| 2 | Preço deve ser **maior que zero** |
| 3 | Não pode existir dois produtos com o **mesmo nome** (ignorar maiúsculas/minúsculas) |
| 4 | Produto **inativo** não pode ser editado (`UpdateAsync` deve falhar) |
| 5 | O `DeleteAsync` é um **soft delete**: apenas define `Ativo = false` |

### 2. `ProdutoRepository.cs` — Infrastructure/Repositories

Implemente as consultas usando **Entity Framework Core** com o banco de dados **InMemory**.

### 3. `ProdutosController.cs` — API/Controllers

Implemente os endpoints REST:

| Método | Rota | Descrição | Status esperado |
|--------|------|-----------|-----------------|
| GET | `/api/produtos?pagina=1&tamanhoPagina=10` | Lista paginada | 200 |
| GET | `/api/produtos/{id}` | Produto por Id | 200 / 404 |
| POST | `/api/produtos` | Criar produto | 201 / 400 |
| PUT | `/api/produtos/{id}` | Atualizar produto | 200 / 400 / 404 |
| DELETE | `/api/produtos/{id}` | Desativar produto | 204 / 404 |

---

## Critério de avaliação

Execute os testes ao finalizar:

```bash
dotnet test
```

**Todos os testes devem passar** (`dotnet test` deve exibir `Passed!`).

Os testes cobrem:
- Busca por Id (existente e inexistente)
- Listagem paginada
- Criação com dados válidos e inválidos (nome vazio, preço zero/negativo, nome duplicado, nome longo)
- Atualização com produto inexistente e produto inativo
- Exclusão (soft delete) com produto existente e inexistente

---

## Dicas

- Leia os comentários `// TODO` em cada arquivo — eles descrevem exatamente o que implementar.
- Use `_notifications.AddNotification("mensagem")` para registrar erros de negócio.
- Retorne `CommandResult<T>.Failure(_notifications.Notifications)` em caso de falha.
- Retorne `CommandResult<T>.Success(dto)` em caso de sucesso.
- O banco de dados já começa populado com categorias e produtos (veja `DataSeeder.cs`).

---

## Entrega

Suba o projeto em um repositório **público** no GitHub e envie o link.

Boa sorte!
