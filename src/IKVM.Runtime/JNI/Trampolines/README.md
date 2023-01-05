The .c files are built to .o files. The .o files are then disassembled to generate .s files. The .s files are then parsed to generate .cs files that declare the function bodies in source.

At runtime these can be loaded into executable memory and invoked.

LLIR code must be PIC without external references.

Entire .text region is mapped into memory, and the offsets to the symbols are converted to pointers in memory. No relocation is done.
