README

The bubble appocalypse - a Pang clone

How to play?

PC\Mac - use either the arrow keys or wasd to move, and the left mouse button or the spaceBar to shoot.
Mobile - drag anywhere to move, tap anywhere to shoot

My work process

First of ll I would like to say that I had a lot of fun creating this game, it was a very nice break from day to day stuff. I tried to utilize the time I had as efficiently as I could, but ended up getting sidetracked on occasion (The new UI system for example). The project was designed using mainly MVC, Observer and some dependency injection. There are a few places that require refactoring. I initially started building the classes on an abstract base but ultimately decided against it because it felt convoluted. I also took a few shortcuts with the MVC pattern, but mainly stuck to it. In addition, I used this opportunity to utilize Unity's new input system. As the new system is event based, none of the classes in the app makes use of the Update method (I used await Task.yield for real-time frame based update). I also tried to utilize the new UI system but ended up ditching it not to waste time. In addition, I ended up not implementing object pooling for the balls due to time constraints but absolutely would implement it.

p.s - The balls and weapon graphics don't look good. I will apply nicer sprites and shaders when I'll get to it.

Extra credit stuff

1) There are 4 levels with increasing difficulty and with different background and music. More levels can be easily added via the GameConfig ScriptableObject. 2) I used custom visuals for the backgrounds and the character. I wanted to use 2D animation for the character but ended up not doing so due to time constraints. 3) The menus and the levels all have their own background music. Other sound effects were not implemented yet. As for the last 2 - dual player game and leaderboard - Left out due to time constraints.

Thank you so much
Hope to hear from you soon

Who do I talk to?

Doron Kanaan tevezisgod@gmail.com 0544-742431
