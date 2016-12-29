# Inspire: an MRI Breathing Game
- This repository is still under construction

This is a game to guide breathing during an MRI. The purpose is to improve image quality and efficiency by encouraging breathing patterns conducive to clear images, especially when the patient is a child. It also improves the patient's experience by providing them entertainment.

At a high level, the application takes data from the bellows attached to the patient and uses it to control the game's character.  The game itself is projected onto a screen that is visible to the patient while in the MRI. Game data is also logged so that it can be analyzed and used in research.

The game itself is very simple.  Inhaling makes the avatar ascend and exhaling causes the avatar to descend, with the purpose being to score as many points as possible by collecting stars. The position of a star is controlled by the game mode and designed to encourage the patient to breath in a certain way.

Technical Notes:
The code requires NSubstitute to compile and run it's unit tests. Unity provides free Testing Tools that will import this Library, otherwise if you are not interested in unit tests you may delete compiler errors rooted from 'Editor' folders without detriment to game functionality.

Contributors:
- Jack Boehrer
- Gibson Hladky
- Zachary Myers
- Michael Spelman
- Nathan Van Hogen
- Aaron Wurtinger-Knaack
- Ruihao Zhu
