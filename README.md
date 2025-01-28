# **Notification System - Rate-Limited Notification Service**

> A service that sends notifications with rate-limiting based on different notification types (Status, News, Marketing) to prevent excessive notifications to users.

---

## **Table of Contents**

1. [Description](#description)
2. [Tech Stack](#tech-stack)
3. [Installation](#installation)
4. [Features](#features)
5. [Usage](#usage)
6. [Tests](#tests)

---

## **Description**

This project is a notification service with rate-limiting functionality. It sends notifications to users based on different types (such as Status, News, Marketing) and enforces limits to ensure users are not overwhelmed with too many notifications in a short period.

The system checks if the number of notifications sent exceeds the defined limit for each message type and rejects new notifications when the limit is reached. It also provides a retry mechanism to ensure notifications are sent whenever possible.

---

## **Tech Stack**

- **.NET 7.0**: Core framework used.
- **FluentAssertions**: For streamlined unit testing and assertions.
- **Moq**: For creating mocks in tests.
- **XUnit**: Testing framework used.
- **AutoFaker**: For generating test data.
- **SwaggerUI**: For API visualization and interaction.

---

## **Installation**

### Prerequisites

Before you begin, ensure you have the following installed:

- [Git](https://git-scm.com/)
- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or your preferred IDE

### Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/your-repository.git
    ```

2. Navigate to the project directory:

    ```bash
    cd your-repository
    ```

3. Restore dependencies:

    ```bash
    dotnet restore
    ```

4. Run the project:

    ```bash
    dotnet run
    ```

5. Run the tests:

    ```bash
    dotnet test
    ```

---

## **Features**

- Send notifications of different types (Status, News, Marketing)
- Rate-limiting based on notification type
- Retry mechanism for notifications blocked due to rate limits
- Cache-based notification history for rate control

---

## **Usage**

To send a notification, call the `Send` method with the required parameters:

```csharp
var notificationBO = new NotificationBO(cacheProvider, gateway);
await notificationBO.Send("news", "user123", "This is a news update.");
