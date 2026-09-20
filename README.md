# DragonCliffMod

龙崖（Dragon Cliff）BepInEx 插件 MOD 骨架。

## 环境要求

- Windows（龙崖是 Windows 游戏）
- [BepInEx 5](https://github.com/BepInEx/BepInEx/releases)（已部署到游戏目录）
- .NET Framework 4.8 SDK（或 Visual Studio 2022+）
- 龙崖 Steam 版

## 快速开始

### 1. 安装 BepInEx

1. 下载 [BepInEx_x64_5.4.22.0.zip](https://github.com/BepInEx/BepInEx/releases)
2. 解压到龙崖游戏根目录（与 `Dragon Cliff.exe` 同级）
3. 首次运行游戏，BepInEx 会自动初始化目录结构
4. 确认 `BepInEx/LogOutput.log` 生成无报错

### 2. 编译插件

```bash
# 克隆本仓库
git clone https://github.com/yehuoshun/DragonCliffMod.git
cd DragonCliffMod

# 编译
dotnet build -c Release
```

### 3. 部署

```bash
# 复制 DLL 到 BepInEx 插件目录
copy bin\Release\net48\DragonCliffMod.dll "Dragon Cliff\BepInEx\plugins\"
```

### 4. 启动游戏

BepInEx 自动加载插件，查看 `BepInEx/LogOutput.log` 确认加载成功。

## 项目结构

```
DragonCliffMod/
├── Plugin.cs                  # BepInEx 入口
├── DragonCliffMod.csproj      # 项目文件
├── Config/
│   └── ModConfig.cs           # 插件配置入口
├── Patches/
│   ├── EquipmentPatch.cs      # 装备相关 patch
│   ├── GemPatch.cs            # 宝石相关 patch
│   ├── EnhancementPatch.cs    # 强化相关 patch
│   └── SkillPatch.cs          # 技能等级相关 patch
└── Properties/
    └── AssemblyInfo.cs
```

## 添加新功能

1. 在 `Patches/` 下新建 `XxxPatch.cs`
2. 用 `[HarmonyPatch]` 标注目标类和方法
3. 实现 `Prefix` / `Postfix` / `Transpiler`
4. 编译部署测试

参考示例：详见各 Patches 目录下的 stub 文件。

## 注意

- 本插件需要引用龙崖的 `Assembly-CSharp.dll`（见 `.csproj` 中的 HintPath 占位）
- 将 `.csproj` 中的 HintPath 改为你本地游戏路径
- 也可通过 `BepInEx/Doorstop.dll` 自动装载，无需额外配置