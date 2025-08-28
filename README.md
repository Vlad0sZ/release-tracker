# Release Tracker

A desktop application built in **Unity 6.0** for Windows, designed to manage and visualize project releases with task tracking and 2D character animations.

## Project Overview

**Release Tracker** is a lightweight, offline Windows application that helps users manage project releases by tracking tasks and visualizing progress through animated sequences. The application features a simple UI for creating and viewing releases, a table for tracking planned vs. actual tasks, and an animation screen with a character moving on a hill to reflect progress.

### Features
- **Start Screen**: Create a new release or view a list of existing releases.
- **Create Release Screen**: Input start date, end date, total tasks, and weekly check-in day.
- **Release View Screen**: Display a table with auto-calculated dates, planned tasks, user-entered actual tasks, and deviation percentage.
- **Animation Screen**: 2D character animations (walking uphill, sliding down, idle) and flag drops based on plan vs. fact data.
- **Data Storage**: Local storage using JSON/PlayerPrefs for persistent release data.
- **Offline**: No internet connection required.

## Requirements
- **Platform**: Windows (64-bit)
- **Unity Version**: 6.0
- **Development Environment**:
  - Unity Editor 6.0
  - C# (Visual Studio or compatible IDE)

## Installation
1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   ```
2. **Open in Unity**:
   - Open Unity Hub.
   - Add the cloned project folder via "Add Project".
   - Ensure Unity 6.0 is installed and selected.
3. **Build the Project**:
   - Go to `File > Build Settings`.
   - Select **Windows** as the platform.
   - Click **Build** to generate the executable.

## Project Structure
- **Assets/**:
  - **Scripts/**: C# scripts for UI, data management, and animations (e.g., `ReleaseData.cs`).
  - **Scenes/**: Unity scenes for Start, Create Release, Release View, and Animation screens.
  - **Sprites/**: 2D assets for character and flag animations.
  - **Prefabs/**: UI elements and reusable game objects.
  - **Data/**: Local JSON files or PlayerPrefs for storing release data.

## Usage
1. **Start Screen**:
   - Click "New Release" to create a new release or select an existing one from the scrollable list.
2. **Create Release Screen**:
   - Enter the start date, planned end date, total number of tasks, and weekly check-in day (e.g., Monday).
   - Save to proceed to the Release View.
3. **Release View Screen**:
   - View the auto-generated table with columns: Date, Plan %, Planned Tasks, Actual Tasks, % Deviation.
   - Enter actual tasks manually to update the table.
   - Click the animation button to view the progress animation.
4. **Animation Screen**:
   - Watch the 2D character walk uphill to the "Plan" point.
   - If actual tasks match the plan, the character stops; if greater, it moves higher; if fewer, it slides down.
   - Flags drop at Plan and Fact points with animations.
   - Click the back button to return to the Release View.

## Technical Details
- **Language**: C# (Unity).
- **UI**: Built with Unity Canvas and TextMeshPro for scalable, responsive interfaces OR Unity UI Toolkit.
- **Animations**: Unity Animator for character states (walk, idle, slide) and DOTween for smooth flag drops and transitions.
- **Data Storage**: JSON serialization or PlayerPrefs for local persistence.
- **Performance**: Optimized for low CPU/GPU usage on Windows desktops.
- **Error Handling**: Input validation for dates and task counts.

## License
This project is licensed. See the `LICENSE` file for details.

## Contact
For issues or suggestions, create an issue in the repository or contact the project maintainer.