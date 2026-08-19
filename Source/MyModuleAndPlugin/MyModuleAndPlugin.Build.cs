// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class MyModuleAndPlugin : ModuleRules
{
	public MyModuleAndPlugin(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] {
			"Core",
			"CoreUObject",
			"Engine",
			"InputCore",
			"EnhancedInput",
			"AIModule",
			"StateTreeModule",
			"GameplayStateTreeModule",
			"UMG",
			"Slate",
			"Temporary"
		});

		PrivateDependencyModuleNames.AddRange(new string[] {"Test" });

		PublicIncludePaths.AddRange(new string[] {
			"MyModuleAndPlugin",
			"MyModuleAndPlugin/Variant_Platforming",
			"MyModuleAndPlugin/Variant_Platforming/Animation",
			"MyModuleAndPlugin/Variant_Combat",
			"MyModuleAndPlugin/Variant_Combat/AI",
			"MyModuleAndPlugin/Variant_Combat/Animation",
			"MyModuleAndPlugin/Variant_Combat/Gameplay",
			"MyModuleAndPlugin/Variant_Combat/Interfaces",
			"MyModuleAndPlugin/Variant_Combat/UI",
			"MyModuleAndPlugin/Variant_SideScrolling",
			"MyModuleAndPlugin/Variant_SideScrolling/AI",
			"MyModuleAndPlugin/Variant_SideScrolling/Gameplay",
			"MyModuleAndPlugin/Variant_SideScrolling/Interfaces",
			"MyModuleAndPlugin/Variant_SideScrolling/UI"
		});

		// Uncomment if you are using Slate UI
		// PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore" });

		// Uncomment if you are using online features
		// PrivateDependencyModuleNames.Add("OnlineSubsystem");

		// To include OnlineSubsystemSteam, add it to the plugins section in your uproject file with the Enabled attribute set to true
	}
}
