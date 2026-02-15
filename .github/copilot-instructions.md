# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction

## Window Management
- Prefer custom window movement by holding the mouse over the TopBar. Implement this by directly moving the window using `SetWindowPos`, `GetCursorPos`, and `SetCapture`, instead of using `SetDragRectangles` or `SendMessage`. This approach allows for a more responsive and intuitive user experience when dragging windows.