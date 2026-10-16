# Tauntastic.ScriptableEnums

Enums as **ScriptableObject assets**. Each value is an asset, so designers can add, rename and extend values without recompiling, and values can carry their own data and methods.

- Package: `com.tauntastic.scriptableenums` (v1.5.0, Unity 2023.2+)
- Assemblies: `Tauntastic.ScriptableEnums` (runtime), `Tauntastic.ScriptableEnums.Editor` (drawers, duplicate guard)
- Namespace: `Tauntastic` (`ScriptableEnum`, attributes), `Tauntastic.ScriptableEnums` (helpers)
- Dependencies: none
- AI agents: see [ARTIFICIAL_INTELLIGENCE_README.md](ARTIFICIAL_INTELLIGENCE_README.md)

## Installation

```
git+https://github.com/exhit/Tauntastic.ScriptableEnums.git
```

## Creating an enum

Derive from the abstract `ScriptableEnum`, add a `CreateAssetMenu`, then create one asset per value. The **asset name** is the value's identifier.

```csharp
using Tauntastic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enums/Rarity")]
public class Rarity : ScriptableEnum
{
    public Color Color = Color.white;   // values can carry data
}
```

## Using it in fields

A `ScriptableEnum` field is drawn as a searchable dropdown of all assets of that type (with ping / open buttons).

```csharp
public class Loot : MonoBehaviour
{
    public Rarity Rarity;                       // dropdown of Rarity assets
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    [ScriptableEnum] public Rarity Minimum;     // force the same drawer on a plain ScriptableObject field
    public ScriptableEnum.Flags<Rarity> Allowed = new(); // multi-select ("flags")
}
```

- `[ScriptableEnum]` - draw any ScriptableObject field as an enum-style dropdown.
- `[ScriptableEnumsDisable]` - disable / grey out a ScriptableEnum field.
- `ScriptableEnum.Flags<T>` - serialized multi-selection; `TypedValues` (`List<T>`), `Values`, `Type`, implicit conversions to `List<T>`, `List<ScriptableEnum>` and `Type`.

## Looking values up

```csharp
List<Rarity> all = ScriptableEnum.GetAll<Rarity>();
Rarity epic     = ScriptableEnum.GetByName<Rarity>("Epic");   // case-insensitive, trimmed; null + warning if 0 or >1 match
ScriptableEnum.SetByName(ref rarity, "Rare");
ScriptableEnum.TrySetIfNullOrWrongName(ref rarity);           // assign the first asset when null
ScriptableEnum.TrySetIfNullOrWrongName(ref rarity, "Common"); // assign by name when null or a different name
```

Also: `GetAll(Type)`, `GetByName(Type, string)`, `GetAllInstances(Type)`, collection / dictionary overloads of `TrySetIfNullOrWrongName`, the extension `this.TrySetIfNullOrWrongName(ref value, name)` (`ScriptableEnumUtils`), and `implicit operator ScriptableEnum(string)` (searches `Resources`, throws on 0 or >1 match).

## Editor vs. builds

- In the **Editor**, `GetAll` uses `AssetDatabase.FindAssets`, so assets can live anywhere.
- In **builds**, it uses `Resources.LoadAll`, so the asset files must be inside a `Resources` folder (or be otherwise included) to be found at runtime. Results are cached per type.
- `PreventProjectWideDuplicateScriptableEnumPostProcessor` watches imported assets and renames project-wide duplicates (same name, same enum type) so identifiers stay unique.

## License

MIT - see [LICENSE](LICENSE).
