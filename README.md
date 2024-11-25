# EventManager 🎉

## Sobre o Projeto
EventManager é uma plataforma web desenvolvida em ASP.NET Core MVC para gerenciamento de eventos. O sistema permite que organizadores criem e gerenciem eventos, enquanto participantes podem visualizar informações e confirmar sua presença, facilitando o controle e organização de eventos.

### Funcionalidades Principais
- 📝 Cadastro de usuários
- 🎫 Criação e gestão de eventos
- 👥 Gestão de participantes
- ✅ Confirmação de presença
- 📊 Controle de lotação
- 📅 Visualização de detalhes do evento

## Tecnologias Utilizadas
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5.3
- BCrypt.Net-Next (para criptografia de senhas)

## Pré-requisitos
- Visual Studio 2022 ou posterior
- .NET 7.0 SDK ou posterior
- SQL Server (LocalDB ou Express)

## Instalação e Configuração

### 1. Clone o Repositório
```bash
git clone https://github.com/seu-usuario/EventManager.git
cd EventManager
```

### 2. Instalação dos Pacotes NuGet
Abra o Package Manager Console no Visual Studio e execute os seguintes comandos:

```powershell
Install-Package BCrypt.Net-Next
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

### 3. Configuração do Banco de Dados
1. Verifique se a string de conexão no `appsettings.json` está correta
2. No Package Manager Console, execute:
```powershell
add-migration InitialCreate
update-database
```

### 4. Configuração do Bootstrap
O projeto utiliza Bootstrap 5.3. O CSS já está incluído no projeto, mas você pode atualizar via CDN ou npm conforme necessário.

## Executando o Projeto

1. Abra a solução no Visual Studio
2. Restaure os pacotes NuGet se necessário
3. Compile o projeto (Ctrl + Shift + B)
4. Execute o projeto (F5)

O aplicativo será iniciado em seu navegador padrão.

## Estrutura do Projeto
```
EventManager/
├── Controllers/ 
│   ├── ConvidadosController.cs
│   ├── EventosController.cs
│   ├── HomeController.cs
│   └── UsuariosController.cs
├── Models/
│   ├── Convidado.cs
│   ├── Evento.cs
│   └── Usuario.cs
│   
├── Views/
│   ├── Convidados/
│   ├── Eventos/
│   ├── Home/
│   └── Usuarios/
└── Data/
    └── GerenciadorEventosContext.cs
```

## Uso
1. Crie uma conta de usuário
2. Faça login no sistema
3. Para criar um evento:
   - Acesse "Criar Evento"
   - Preencha as informações necessárias
   - Configure as opções de participação
4. Para participar de um evento:
   - Busque o evento desejado
   - Visualize os detalhes
   - Preencha as informações necessárias
   - Confirme sua presença

## Contribuindo
1. Faça um Fork do projeto
2. Crie uma Branch para sua Feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a Branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## Licença
Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## Contato
Halbert Nascimento - [halbertfsa@gmail.com](mailto:halbertfsa@gmail.com)

Link do Projeto: [https://github.com/Halbert-Nascimento/EventManager](https://github.com/Halbert-Nascimento/EventManager)
