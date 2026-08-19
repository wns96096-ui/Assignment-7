// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "TemporaryCharacterData.generated.h"

/**
 * 
 */
UCLASS(BlueprintType, Blueprintable)
class TEMPORARY_API UTemporaryCharacterData : public UObject
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Character Data")
	FString CharacterName = TEXT("Temporary Hero");

	UPROPERTY(
		EditAnywhere,
		BlueprintReadWrite,
		Category = "Character Data",
		meta = (ClampMin = "1")
	)
	int32 MaxHealth = 100;
};
