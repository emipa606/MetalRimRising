# Metal Rim Rising (Continued) - Copilot Instructions

## Mod Overview and Purpose
**Metal Rim Rising (Continued)** is a mod designed to enhance the RimWorld experience by integrating elements of "memes" from the game Metal Gear Rising: Revengeance. Developed by the aRatoCat team, this mod focuses on introducing high armor penetration weapons and specialized bionics with unconventional abilities that go beyond basic stat increases. Although some features can make pawns exceptionally resilient, they aim to offer unique gameplay mechanics rather than just power-boosting.

## Key Features and Systems
- **Weapons with High Armor Penetration**: Many new weapons have excellent armor penetration but are balanced with low damage or speed.
- **Specialized Bionics and Equipment**: Introduces bionics that offer interesting abilities designed to enhance gameplay variety. Some items deliberately create formidable pawns for thematic consistency with Metal Gear Rising: Revengeance.
- **Custom Apparel and Equipment**: Exo frames that offer unique properties and interactions.
- **Unconventional Abilities**: Items and bionics introduce abilities that extend beyond typical damage increases, focusing on creative utility and interaction in the game world.

## Coding Patterns and Conventions
- **Class Implementation**: Each game object (weapon, apparel) and ability is encapsulated within classes such as `MGR_ExoFrame`, `Effect_NanomachineCore`, and `HediffCompAdjustPower`.
- **Method Structures**: Core methods include effect application (e.g., `AbsorbedDamage`, `CastingEffect`) and game logic operations (e.g., `Reset`, `Break`).
- **Naming Conventions**: Classes and methods adopt a consistent prefix (`MGR_`, `Effect_`, `Hediff_`) to categorize and streamline code organization.

## XML Integration
XML files are used extensively for configuration and balancing. These files define weapon stats, bionic attributes, and trait interactions. As noted, players are encouraged to customize XML configurations for personal game balance preferences.

## Harmony Patching
The mod utilizes Harmony for patching core game mechanics to introduce new features and adjust existing content. Harmony allows for non-intrusive modifications, ensuring compatibility with future game updates and reducing conflicts with other mods. 

## Suggestions for Copilot
- **Automation of XML Updates**: Leverage Copilot to generate template XML snippets for new weapons or bionics, ensuring correct schema adherence.
- **Code Refactoring**: Suggest simplifications or optimizations in existing methods, especially in complex logic operations like `DamageEntities` and `SearchForTargets`.
- **Pattern Recognition**: Detect common class patterns and offer auto-completions for recurring code structures (e.g., patches, effect triggers).
- **Error Handling**: Propose error and exception handling practices, especially relevant to known issues like exo-suit detonations.

## Known Issues
The mod has some known issues, such as the D.E.S Exo suit sometimes not detonating and interaction conflicts with the Vanilla Shield Belt. These issues require careful debugging, possibly through additional logging and testing with Harmony patches. Code comments and logging improvements can aid in this troubleshooting process.

## Compatibility
This mod is not compatible with "Combat Extended," a popular RimWorld mod known for its comprehensive combat overhaul.

## Feedback and Contact
For any bugs or queries, users are encouraged to provide feedback through Hugslib logs and screenshots to assist in improving the mod's performance and compatibility. 

Developers can use these instructions to create new features, maintain consistency, and refine existing codebases while leveraging GitHub Copilot for efficient development.
