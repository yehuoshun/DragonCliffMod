# DragonCliffMod

龙崖（Dragon Cliff）BepInEx 插件 MOD 骨架。

## 环境要求

- Windows（龙崖是 Windows 游戏）
- [BepInEx 5 **x86**](https://github.com/BepInEx/BepInEx/releases)（已部署到游戏目录，x64 版无法注入 32 位游戏）
- .NET SDK（用于 `dotnet build`）
- 龙崖 Steam 版

> ⚠️ 龙崖是 **Unity 5.6.6 + .NET 3.5**（CLR 2.0.50727），插件必须 target **net35**，不要用 net48。

## 快速开始

### 1. 安装 BepInEx 5 x86

1. 下载 `BepInEx_x86_5.4.22.0.zip`（**x86 不是 x64**）
2. 解压到龙崖游戏根目录，使 `winhttp.dll` 与 `game.exe` 同级
3. 启动一次游戏，确认 `BepInEx/LogOutput.log` 生成，出现 `CLR runtime version: 2.0.50727` 即注入成功

### 2. 编译插件

```bash
git clone https://github.com/yehuoshun/DragonCliffMod.git
cd DragonCliffMod

# 改 csproj 里的 DragonCliffDir 指向你的游戏目录（默认 F 盘）

dotnet build -c Release
```

### 3. 部署

```bash
copy bin\Release\net35\DragonCliffMod.dll "Dragon Cliff\BepInEx\plugins\"
```

### 4. 启动游戏

看 `BepInEx/LogOutput.log`，出现 `龙崖 MOD 加载中...` 即成功。

配置项自动生成在 `BepInEx/config/yehuoshun.DragonCliffMod.cfg`。

## 项目结构

```
DragonCliffMod/
├── Plugin.cs                  # BepInEx 入口（含 MyPluginInfo）
├── DragonCliffMod.csproj      # net35 项目，引用游戏目录真实 DLL
├── Config/
│   └── ModConfig.cs           # 插件配置入口
├── Patches/
│   ├── EquipmentPatch.cs      # 装备相关 patch（示例，默认禁用）
│   ├── GemPatch.cs            # 宝石相关 patch（示例，默认禁用）
│   ├── EnhancementPatch.cs    # 强化相关 patch（示例，默认禁用）
│   └── SkillPatch.cs          # 技能等级相关 patch（示例，默认禁用）
└── Utils/
    └── Extensions.cs
```

## 添加新功能

1. 反编译 `Assembly-CSharp.dll` 拿到目标类名/方法名/namespace
2. 在 `Patches/` 下新建或编辑 `XxxPatch.cs`
3. 用 `[HarmonyPatch]` 标注目标类和方法，实现 `Prefix`/`Postfix`/`Transpiler`
4. 把 `#if false` 改为 `#if true` 启用
5. `dotnet build` → 部署 → 测试

## 关键注意

- **Unity 5.6.6 没有模块化 UnityEngine**，不能引用 `UnityEngine.Modules`，直接引用 `game_Data/Managed/UnityEngine.dll`
- **存档序列化用 ZeroFormatter**（`ZeroFormatter.dll`），涉及存档字段的改动要格外小心
- 所有 patch 默认 `#if false` 禁用，骨架本身能编译通过；逐个解开启用
