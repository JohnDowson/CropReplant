<!-- HEADER -->
# CropReplant
A handy quality of life mod for Valheim.

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]

<!-- DOWNLOAD -->
# Download
You can download the latest version of the mod from [NexusMods](https://www.nexusmods.com/valheim/mods/99?tab=files).

<!-- USAGE -->
# Usage
There are two ways you can use CropReplant.

<!-- OPTION 1 -->
### Option 1: Middle Click (Default)
The following is the default behavior for CropReplant:

1) Equip a cultivator.
2) Point crosshair at a fully grown crop.
3) Right click, Left click a plant you want to plant/replant in the cultivator build menu
4) Right click again, then Middle mouse click the plant you want to replant *while still in the cultivator build menu*.

**WARNING!** 
> You must middle click the plant portrait within the cultivator build menu otherwise you'll accidentally kill the targeted/nearby crop!

<!-- OPTION 2 -->
### Option 2: Key Binding
Some may find this approach faster or more intuitive. You can edit the configuration for CropReplant to use a key binding. 

The configuration file can be found in the Valheim's install location:  
```
<Valheim-Install>\BepInEx\config\com.github.Ch0z.CropReplant.cfg
```

- Update the `UseCustomReplantKey` property to `true`.  
- By default, the key binding is set to `H` as in previous releases.
- You may also assign a custom key by updating the value of the `CustomReplantKey` property.  
[Click here](https://docs.unity3d.com/ScriptReference/KeyCode.html) for a full list of supported key codes you use.

Now you can use the following method to replant:

1) Select the crop to replant in the cultivator build menu (right click)
2) Point crosshair to any fully grown crop
3) Press `CustomReplantKey` (`H` by default)

> Using the key binding method doesn't need you to go into the cultivator build menu twice!

<!-- CONTRIBUTING -->
## Contributing
If you are a developer and understand how to use Git, feel free to submit a PR for review.

<!-- LICENSE -->
## License
Distributed under the GNU Affero General Public License v3.0. See [LICENSE](https://github.com/JohnDowson/CropReplant/blob/main/LICENSE) for more information.

<!-- STYLE LINKS -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/JohnDowson/CropReplant.svg?style=plastic
[contributors-url]: https://github.com/JohnDowson/repo/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/JohnDowson/CropReplant.svg?style=plastic
[forks-url]: https://github.com/JohnDowson/repo/network/members

[stars-shield]: https://img.shields.io/github/stars/JohnDowson/CropReplant?style=plastic
[stars-url]: https://github.com/JohnDowson/CropReplant/stargazers

[issues-shield]: https://img.shields.io/github/issues/JohnDowson/CropReplant?style=plastic
[issues-url]: https://github.com/JohnDowson/CropReplant/issues
