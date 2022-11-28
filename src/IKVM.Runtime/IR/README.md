The idea here is that we compile the C source to LLIR and commit the LLIR to source control. This way we don't need headers to generate the functions.

The .ll files are built to .o files. The .o files are then disassembled to generate .s files. The .s files are then parsed to generate .cs files that declare the function bodies in source.

At runtime these can be loaded into executable memory and invoked.
