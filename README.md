# Maze Game Server

## API design
BASE_URL: `https://maze.nerd.ink/api`

- `/mazes`: 301 response redirecting to `/api`
- `/mazes/random`: generates a random maze and returns it as JSON, already possible on the client side... not sure if that's useful.
- `/mazes/:id`: returns a maze

Only mazes generated by the server have IDs, or mazes sent to the server, which are then given IDs


Verifying a maze route:
256 characters
UDNSEWD

Needs to provide