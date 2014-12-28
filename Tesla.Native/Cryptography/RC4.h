/** @file: RC4.h */

#include <stdint.h>

void RC4_Init(uint8_t* key, uint8_t length);
void RC4_Reset(void);
void RC4_Transform_Full_Direct(uint8_t* data, size_t offset, size_t length);
void RC4_Transform_Block_Direct(uint8_t* data, size_t ffset, size_t length);
void RC4_Dispose(void);
