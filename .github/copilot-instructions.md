# GitHub Copilot Instructions for Metal Rim Rising (Continued)

Welcome to the Metal Rim Rising (Continued) mod project! This file provides instructions and guidelines to help Copilot users contribute effectively. 

## Mod Overview and Purpose

Metal Rim Rising is a mod inspired by Metal Gear Rising: Revengeance, developed by the aRatoCat team. It enhances RimWorld with new weapons, bionics, and equipment focused primarily on armor penetration. The mod aims to introduce unique abilities rather than simple stat increases, thus adding a "meme" and thematic element to the game. 

## Key Features and Systems

- **High Armor Penetration Weapons**: Most weapons have a high armor penetration value while balancing with lower damage or slower speed.
- **Specialized Bionics and Equipment**: Provides unique abilities, including bionics that can make a pawn extremely resilient.
- **Known Issues**: Some functionality like the D.E.S Exo suit and Vanilla Shield Belt may not work as intended in specific cases.

## Coding Patterns and Conventions

- Classes are declared as internal unless they need to be accessed publicly (__e.g.,__ `public class MGRKatana`).
- Methods tend to be private if used only within their defining class.
- Standard C# naming conventions are used: PascalCase for classes and methods.

## XML Integration

XML files are used extensively for defining items, weapons, and their properties. You can easily modify attributes such as bionic power levels:

- Example XML path: `Defs/Items/YourItem.xml`
- Be cautious when editing XML files, as incorrect values may lead to imbalanced gameplay or errors.

## Harmony Patching

Harmony is used for patching methods to modify base game behavior:

- Aim to patch methods judiciously to minimize conflicts with other mods.
- Follow the pattern of prefix, postfix, and transpiler methods, only altering features that need modification.

## Suggestions for Copilot

When using GitHub Copilot to assist with this project, consider the following suggestions:

- **Code Readability**: Suggest enhancing comments for complex code blocks to improve clarity.
- **Maintainability**: Encourage modular design, where new features or patches do not disrupt existing functionality.
- **Error Handling**: Include thorough logging in patches or methods, especially where known issues exist.
- **XML Editing Aid**: Provide auto-complete suggestions for XML tags commonly used in this mod, such as `<WeaponDef>` or `<BionicDef>`.

For effective debugging and contributions, make sure to use the HugsLib logs and contact the team through the Discord channel for support.

Happy modding, and enjoy contributing to Metal Rim Rising!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.
