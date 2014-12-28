/** @file: RC4.c */

#include "RC4.h"
#include <stdlib.h>
#include <stdio.h>
#include <memory.h>

uint8_t* _originalKey;
size_t _originalKeyLength;

uint8_t _s[256];

void RC4_Swap(uint8_t* array, size_t idx1, size_t idx2)
{
	uint8_t t = array[idx1];
	array[idx1] = array[idx2];
	array[idx2] = t;
}

void RC4_KeySchedule(void)
{
	size_t i, j;

	for (i = 0; i < 256; i++)
	{
		_s[i] = (uint8_t) i;
	}

	for (i = 0, j = 0; i < 256; i++)
	{
		j = (j + _s[i] + _originalKey[i % _originalKeyLength]) % 256;
		RC4_Swap(_s, i, j);
	}
}

void RC4_Reset(void)
{
	RC4_KeySchedule();
}

void RC4_Init(uint8_t* key, size_t length)
{
	_originalKey = malloc(length);
	_originalKeyLength = length;

	memcpy(_originalKey, key, length);
	RC4_Reset();
}

uint8_t RC4_KWord(size_t i, size_t j)
{
	uint8_t k;
	
	i = (i + 1) % 256;
	j = (j + _s[i]) % 256;

	RC4_Swap(_s, i, j);
	k = _s[(_s[i] + _s[j]) % 256];

	return k;
}

void RC4_Transform_Block_Direct(uint8_t* data, size_t offset, size_t length)
{
	size_t i, j;
	uint8_t k;

	for (i = offset, j = 0; i < offset + length; i++)
	{
		k = RC4_KWord(i, j);
		data[i] = (data[i] ^ k);
	}
}

void RC4_Transform_Full_Direct(uint8_t* data, size_t offset, size_t length)
{
	size_t i, j;
	uint8_t k;

	RC4_Reset();

	for (i = offset, j = 0; i < offset + length; i++)
	{
		k = RC4_KWord(i, j);
		data[i] = (data[i] ^ k);
	}
}

void RC4_Dispose(void)
{
	free(_originalKey);
}
