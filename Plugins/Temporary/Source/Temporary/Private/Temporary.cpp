// Copyright Epic Games, Inc. All Rights Reserved.

#include "Temporary.h"
#include "CoreMinimal.h"

#define LOCTEXT_NAMESPACE "FTemporaryModule"

void FTemporaryModule::StartupModule()
{
	 UE_LOG(LogTemp, Warning, TEXT("[Temporary] StartupModule"));
}

void FTemporaryModule::ShutdownModule()
{
UE_LOG(LogTemp, Warning, TEXT("[Temporary] ShutdownModule"));
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FTemporaryModule, Temporary)