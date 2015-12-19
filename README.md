# Reversi with Unity3d

![Initial Screen](https://github.com/dodikk/reversi-unity3d/raw/master/images-readme/1.png)
![After some turns](https://github.com/dodikk/reversi-unity3d/raw/master/images-readme/2.png)

This is my very first game with unity3d (v5.3.0).
If you have any improvement suggestions, feel free describing them in github issues.

```
License : MIT
```

# Features Implemented
1. Board and figure visuals (simple cube and sphere GameObject instances)
2. Valid turns highlighting
3. Simple AI using a "greedy" strtegy (the more figures flipped - the better)
4. Set the ```IS_OPPONENT_PLAYER_AI = false``` variable to play Human vs. Human. No programmatic switch has been implemented.



# Implementation notes

1. The implementation stages can be viewed in the branches.
2. A tag has been created for each milestone 
3. Initial board state painted in the editor
4. Materials have been borrowed from the [learning geek blog](https://learninggeekblog.wordpress.com/2013/04/29/chess-part1-board-and-basic-movement/)
5. The ```./Reversi/ReversiKit``` directory contains a solution for testing business logic
6. Actual files are located in the ```./Reversi/Assets/ReversiKit``` directory in order to be recognized by Unity properly
7. ```#if NO_UNITY``` preprocessor has been used to disable [Guards.NET](https://www.nuget.org/packages/Guards.NET/) library in unity build. I have not figured out a way to make Unity work with NuGet
8. No performance/memory footprint optimization has been done.
9. No optimization of visual effects has been done.
