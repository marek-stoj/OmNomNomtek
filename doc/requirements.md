# Requirements

+ Create a public git repository e.g. on Github and commit the template along with changes that you make to complete the project.
+ As an overview, the experience should consist of a list of objects that can be placed in the scene in play mode and interacted with using Unity’s physics engine.
  + There’s no need to make a build that runs on any specific platform. Running the app from the editor is just fine.
- A scrollable grid/list implemented using Unity UI should be anchored to the left edge of the screen with three types of placeable cubes (red, green, and blue)
  + Every list item has a picture and a title.
  - The thumbnail image can simply be a screenshot of the object as seen in the editor.
  + The UI list should be populated based on a collection of elements defined as an asset that can be modified independently from a scene and reused between different scenes.
  + Each item in the asset should be defined as a title and a content asset.
- At the top of the list, there should be a text field acting as a filter.
  - As we enter or remove subsequent characters, only objects with titles matching the filter (case insensitive) should be displayed on the list.
  - The list should be filtered on the fly as we type.
+ At the center of the scene, there is a large, thin cube imitating the floor.
- When we click on a list item:
  - The list should hide with a short move animation (please use the DOTween library for the animation) beyond the left edge of the screen.
  + An object based on the selected item should be spawned and start following the cursor.
  - As we move the cursor, the object should keep snapping to the floor at the location currently indicated by the cursor.
  - When the user left-clicks while moving the object:
    + It should be placed at the location indicated by the cursor.
    - The list should reappear with an animation reverse of the hide animation.
  - If the user presses ESC instead of clicking, the object should be destroyed, and the list should reappear.
+ All cubes should have a rigid body and react physically after being placed in the scene (gravity, collisions with other rigid bodies).
+ The object "carried" by the cursor should physically interact with the cubes already placed in the scene, e.g., we should be able to push the already placed cubes around with the “carried” cube.
  + The carried cube’s position should be controlled only by the script, e.g., when it collides with other cubes, those cubes should move, but the carried cube should continue following the cursor.
- Finally, we should add one last element type to the list, a yellow ball (Cube Eater) with some visual element showing the direction it’s facing.
  - When selected from the list, the ball should be placeable in the same way as the cubes.
  - Once placed in the scene, it should move and rotate towards the nearest placed cube (it shouldn’t follow a “carried” cube) with a speed of movement and rotation set via the inspector.
  - When it collides with the cube, it should destroy it and move toward the next one.
  - If there are no more cubes, the ball should stop moving.
  - As long a Cube Eater is “carried” it should not eat cubes already on the floor.
