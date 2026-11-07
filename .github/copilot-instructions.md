# GitHub Copilot Instructions for Metal Rim Rising (Continued)

## Mod Overview and Purpose
Metal Rim Rising (Continued) is a mod created by the aRatoCat team to add more "memes" to Rimworld. The mod primarily focuses on armor penetration, where most weapons added to the mod have high armor penetration values but often low damage or low speed. Bionics and equipment are more specialized, allowing for more interesting abilities that aren't simply a number increase. Although some are simply there to make your pawn feel f-cking invincible. The mod is a continuation of crazedmonkey231's original mod.

## Key Features and Systems
- **Armor Penetration**: Most weapons added to the mod have high armor penetration values but often low damage or low speed.
- **Bionics and Equipment**: Specialized bionics and equipment are added that provide interesting abilities, but some may make pawns feel invincible.
- **XML Integration**: The mod uses XML files to define new things, effects, hediffs, and graphics.
- **Harmony Patching**: The mod uses Harmony patching to add new functionality or modify existing functionality in Rimworld.

## Coding Patterns and Conventions
- **C#**: The mod uses C# for defining new things, effects, hediffs, and graphics.
- **XML**: The mod uses XML for defining new things, effects, hediffs, and graphics.
- **Harmony**: The mod uses Harmony patching to add new functionality or modify existing functionality in Rimworld.
- **Code Comments**: The mod includes code comments to explain the purpose of code sections.
- **Code Formatting**: The mod follows standard code formatting conventions.

## XML Integration
The mod uses XML files to define new things, effects, hediffs, and graphics. Here is an example of an XML file that defines a new weapon:
xml
<ThingDef>
  <defName>RaidenSword</defName>
  <label>Raiden Sword</label>
  <description>A high-armor-penetrating sword with low damage.</description>
  <category>Weapon</category>
  <thingClass>ThingWithComps</thingClass>
  <statBases>
    <AttackDamage>10</AttackDamage>
    <ArmorPenetration>5</ArmorPenetration>
  </statBases>
  <comps>
    <li Class="CompProperties_VerbGiver">
      <verbs>
        <li>
          <verbClass>Verb_Use</verbClass>
          <warmupTime>0</warmupTime>
          <castTime>0.5</castTime>
          <recoilTime>0.1</recoilTime>
          <range>1.5</range>
          <verbProps Class="VerbProperties_AreaAttack">
            <areaRadius>0.5</areaRadius>
          </verbProps>
        </li>
      </verbs>
    </li>
  </comps>
</ThingDef>


## Harmony Patching
The mod uses Harmony patching to add new functionality or modify existing functionality in Rimworld. Here is an example of a Harmony patch that adds a new verb to a weapon:
csharp
[HarmonyPatch(typeof(Verb_Use), "TryCastNextTarget", typeof(LocalTargetInfo))]
public static class Verb_Use_TryCastNextTarget_Patch
{
  [HarmonyPostfix]
  public static void Postfix(Verb_Use __instance, LocalTargetInfo target)
  {
    if (__instance.parent.def.defName == "RaidenSword")
    {
      // Add new verb to the weapon
      __instance.parent.TryAddVerb(new Verb_Use
      {
        verbClass = typeof(Verb_Use),
        warmupTime = 0,
        castTime = 1,
        recoilTime = 0.2,
        range = 2,
        verbProps = new VerbProperties_AreaAttack
        {
          areaRadius = 1
        }
      });
    }
  }
}


## Suggestions for Copilot
- **XML Integration**: When generating XML files, include comments to explain the purpose of each element.
- **Harmony Patching**: When generating Harmony patches, include comments to explain the purpose of each patch and any potential side effects.
- **Code Formatting**: Follow standard code formatting conventions to ensure code is easy to read and understand.
- **Code Comments**: Include code comments to explain the purpose of code sections to improve code readability.
- **Error Handling**: Include error handling code to ensure the mod is robust and does not crash the game.

By following these guidelines, you can help ensure that the Metal Rim Rising (Continued) mod is high-quality, easy to read, and robust.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

