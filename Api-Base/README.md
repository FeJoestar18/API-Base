# Api-Base

Instruções rápidas para configurar e rodar o projeto localmente.

Pré-requisitos
- .NET 8 SDK
- MySQL local ou acessível

Setup (desenvolvimento)

1. Copie o arquivo de exemplo `.env.sample` para `ApiBase.API/.env` e preencha as variáveis (ou use variáveis de sistema):

   cp ApiBase.API/.env.sample ApiBase.API/.env

2. Instale dependências e compile a solução:

   dotnet restore
   dotnet build

3. (Opcional) Crie/migre o banco de dados a partir do projeto `ApiBase.Infrastructure` usando `dotnet ef` (instale `dotnet-ef` globalmente se necessário):

   dotnet tool install --global dotnet-ef
   dotnet ef migrations add InitialCreate -p ApiBase.Infrastructure -s ApiBase.API
   dotnet ef database update -p ApiBase.Infrastructure -s ApiBase.API

4. Rode a API:

   cd ApiBase.API
   dotnet run

Variáveis de ambiente suportadas (em ordem de prioridade):
- CONNECTION_STRING — usa a string completa se definida
- DB_HOST, DB_PORT, DB_USERNAME (ou DB_USER), DB_PASSWORD, DB_NAME — montam a connection string
- Jwt:Key ou JWT_KEY — chave utilizada para assinar tokens JWT

Produção
- Não utilize `.env` em produção. Prefira secret managers (Azure Key Vault, AWS Secrets Manager) ou variáveis de ambiente do ambiente de execução.
- Defina explicitamente a versão do servidor MySQL em `Program.cs` se quiser evitar AutoDetect.

Se preferir, eu posso:
- adicionar um health-check endpoint que verifica a conexão com o banco,
- criar um `docker-compose.yml` com MySQL e a API para facilitar testes locais.

