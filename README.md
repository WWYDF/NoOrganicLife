## Description
Makes organic life on Embrion not spawn like the logs suggest. 

**Credits:** X-Referenced some code from [No-Monsters](https://github.com/Angel-Madeline/No-Monsters).

![Embrion Terminal](img/EmbrionTerminal.png?raw=true "Title")

## Development
I used Visual Studio 2022 to write this. In the Project File, change `<Profile>NoOrganicLife</Profile>` to be the name of your r2modman/TSMM profile. Mine in this case is `NoOrganicLife`.

Make sure to change your `<GameDirectory>` key if Lethal Company is not installed in the default location! By default, this is `C:\Program Files (x86)\Steam\steamapps\common\Lethal Company`.

## Config
By default, Mechs are enabled, and cannot be disabled. There are some options to enable other mobs as well.

| Key         | Enemies                    | Default |
|-------------|----------------------------|---------|
| allowBlobs  | Hygrodere                  | true    |
| allowLiving | Coilhead Nutcracker Jester | true    |