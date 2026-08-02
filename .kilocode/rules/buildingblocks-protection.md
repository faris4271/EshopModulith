---
paths:
  - "Src/BuildingBlocks/**/*"
---

# ⚠️ BuildingBlocks Protection

**STOP. You are modifying BuildingBlocks.**

Changes to BuildingBlocks affect ALL modules across the entire framework. These are core abstractions that many projects depend on.

## Before Proceeding

1. **Confirm explicit approval** - Has the user specifically approved this change?
2. **Consider alternatives** - Can this be done in the module instead?
3. **Assess impact** - What modules will this affect?

## If Approved

- Make minimal, focused changes
- Ensure backward compatibility
- Update all affected modules
- Run full test suite: `dotnet test EshopModulith.slnx`
- Document the change

## Alternatives to Consider

| Instead of... | Consider... |
|---------------|-------------|
| Modifying `Shared` | Extension method in module |
| Editing `Shared/Abstraction` or `Shared/Persistence` (folders inside the `Shared` package — there is no separate `Shared.Persistence` package) | Module-local repository / extension method |
| Updating Shared.Web | Module-specific middleware |

## If Not Approved

Do not proceed. Suggest alternatives that don't require BuildingBlocks modifications.
