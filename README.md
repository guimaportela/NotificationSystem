# **Notification System - Rate-Limited Notification Service**

> A service that sends notifications with rate-limiting based on different notification types (Status, News, Marketing) to prevent excessive notifications to users.

---

## **Índice**

1. [Descrição](#descrição)
2. [Funcionalidades](#funcionalidades)
3. [Tecnologias Usadas](#tecnologias-usadas)
4. [Instalação](#instalação)
5. [Como Usar](#como-usar)
6. [Testes](#testes)
7. [Contribuição](#contribuição)
8. [Licença](#licença)

---

## **Descrição**

Este projeto é um serviço de notificação com limitação de taxa (rate-limiting). Ele envia notificações para os usuários com base em diferentes tipos (como Status, News, Marketing) e aplica limites para garantir que os usuários não sejam sobrecarregados com muitas notificações em um curto período de tempo.

O sistema verifica se o número de notificações enviadas excede o limite definido para cada tipo de mensagem e rejeita novas notificações quando o limite é atingido. Ele também oferece uma funcionalidade de retry para garantir que as notificações sejam enviadas quando possível.

---

## **Funcionalidades**

- Envio de notificações de diferentes tipos (Status, News, Marketing)
- Rate-limiting baseado em diferentes tipos de notificação
- Retentativa de envio de notificações quando o limite de taxa é atingido
- Armazenamento de histórico de notificações no cache para controle de limites

---

## **Tecnologias Usadas**

- **.NET 8.0**: Framework principal utilizado.
- **FluentAssertions**: Para facilitar os testes unitários e asserções.
- **Moq**: Para criação de mocks nos testes.
- **XUnit**: Framework de testes utilizado.
- **AutoFaker**: Para geração de dados de teste.
- **GitHub Actions**: Para automação de CI/CD.

---

## **Instalação**

### Pré-requisitos

Antes de começar, você precisará de:

- [Git](https://git-scm.com/)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet)
- [Visual Studio](https://visualstudio.microsoft.com/) ou outro editor de sua escolha

### Passos

1. Clone o repositório:

    ```bash
    git clone https://github.com/seu-usuario/seu-repositorio.git
    ```

2. Navegue até o diretório do projeto:

    ```bash
    cd seu-repositorio
    ```

3. Restaure as dependências:

    ```bash
    dotnet restore
    ```

4. Execute o projeto:

    ```bash
    dotnet run
    ```

5. Para rodar os testes:

    ```bash
    dotnet test
    ```

---

## **Como Usar**

Para enviar uma notificação, chame o método `Send` passando os parâmetros necessários:

```csharp
var notificationService = new NotificationService();
await notificationService.Send("news", "user123", "This is a news update.");
