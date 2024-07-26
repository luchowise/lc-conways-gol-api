# LC Conway's Game of Life API

This API is for Conway's Game of Life, a cellular automaton created by John Horton Conway. The API lets you upload an initial board state, get the next state, get the state after a number of generations, and get the final state after a maximum number of iterations.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
  - [Upload Board State](#upload-board-state)
  - [Get Next State](#get-next-state)
  - [Get States Away](#get-states-away)
  - [Get Final State](#get-final-state)
- [Docker](#docker)
  - [Build Docker Image](#build-docker-image)
- [Configuration](#configuration)

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/luchowise/lc-conways-gol-api.git
    ```

2. Go to the project directory:
    ```bash
    cd lc-conways-gol-api
    ```

3. Build the project:
    ```bash
    dotnet build
    ```

4. Run the project:
    ```bash
    dotnet run
    ```

The API will be available at `http://localhost:5000`.

## Usage

You can use tools like Postman, Insomnia or cURL to consume and test the API. Below are some examples of how to use every endpoint.

## API Endpoints

### Upload Board State

Uploads the initial board state.

- **URL:** `/api/game-of-life/upload-board-state`
- **Method:** `POST`
- **Content-Type:** `application/json`
- **Request Body:**
    ```json
    {
        "board": [
            [0, 1, 0],
            [0, 0, 1],
            [1, 1, 1]
        ]
    }
    ```

- **Response:**
    ```json
    {
        "id": 1
    }
    ```

### Get Next State

Gets the next state of the board.

- **URL:** `/api/game-of-life/next-state/{id}`
- **Method:** `GET`

- **Response:**
    ```json
    {
        "id": 1,
        "board": [
            [0, 0, 0],
            [1, 0, 1],
            [0, 1, 1]
        ]
    }
    ```

### Get States Away

Gets the state of the board after a given number of generations.

- **URL:** `/api/game-of-life/states-away/{id}/{n}`
- **Method:** `GET`

- **Response:**
    ```json
    {
        "id": 1,
        "board": [
            [0, 0, 0],
            [1, 0, 1],
            [0, 1, 1]
        ]
    }
    ```

### Get Final State

Gets the final state of the board after a max number of iterations.

- **URL:** `/api/game-of-life/final-state/{id}/{maxAttempts}`
- **Method:** `GET`

- **Response:**
    ```json
    {
        "id": 1,
        "board": [
            [0, 0, 0],
            [0, 1, 0],
            [0, 1, 0]
        ]
    }
    ```

## Docker

### Build Docker Image

To build a Docker image for the API, run the following command in the directory containing the `Dockerfile`:

```bash
docker build -t lcconwaygol-api .
```

## Configuration

To configure the file path for storing the board states, you need to update the `appsettings.json` file. Add the below entry to the `appsettings.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BoardStatesFilePath": "the/path/to/the/states.json"
}