using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using MAtrixMultiplication;

namespace MatrixMultiplication
{
    public partial class App : Application
    {
        [DllImport(@"C:\Users\ASUS\OneDrive\Pulpit\MAtrixMultiplication\x64\Debug\MatrixMultiplicationAsm.dll")]
        static public extern unsafe int  AsmMultiplication(int* resultRow, int* row, int* column, int* args);

        [DllImport(@"C:\Users\ASUS\OneDrive\Pulpit\MAtrixMultiplication\x64\Debug\MatrixMultiplicationCpp.dll")]
        public static extern unsafe void CppMultiplication(int* resultRow, int* row, int* column, int columns, int rows);
    }

}
/*
  .CODE

;-------------------------------------------------------------------------
;-------------------------------------------------------------------------

AsmMultiplication PROC loopCount: qword, secondLoopCount: qword, startColAddress : qword, startRowAddress : qword, count : qword, columnsAfterAlign : qword, tmp : qword, columns : qword                                                                                                                                                               
						; resultRow in RCX
						; rowToMultiply in RDX
						; colToMultiply in R8
						; args in R9

mov EAX, [R9]
mov secondLoopCount, RAX

add R9, 4

mov EAX, [R9]
mov columns, RAX
mov loopCount, RAX


add R9, 4
mov EAX, [R9]      ; matrixSize = columnsAfterAlign
mov columnsAfterAlign, RAX
mov tmp, RAX


mov count, 0

mov R10, RDX
mov R9, RCX

mov startColAddress, R8
mov startRowAddress, R10

loop1:
mov R8, startColAddress				; column = startColAddress
mov R10, startRowAddress			; row = startRowAddress

mov RAX, count						; |
shl RAX, 2							; |
add R8, RAX							; |	column += i

xor RAX, RAX						; |
mov [R9], EAX						; (*resultRow) = 0 RAX

mov RAX, columns					; |
mov loopCount, RAX					; |
pxor xmm7, xmm7						; | preparing for multiplying in loop2
inc count
	
			loop2:
			movdqu xmm0, [R10]					;move actual row element to vector ; bylo movq
			movss xmm4, dword ptr [R8]			;move actual column element to vector ; bylo movq
			shufps  xmm4,xmm4,00000000b
			movdqu xmm3, xmm4
			pmulld xmm0, xmm3
			haddps xmm2, xmm0
			pxor xmm6, xmm6	
			haddps xmm6, xmm2
			unpckhpd xmm6, xmm6
			paddq xmm7, xmm6
			
			

			add R10, 16							; row++ bylo 4

			mov RAX, columns	 				; |
			shl RAX, 2							; | bylo 2
			add R8, RAX							; | column += size 

			mov RDX, loopCount					; |
			dec RDX								; | decrementing loop counter
			dec RDX								; |
			dec RDX								; |
			dec RDX								; |
			mov loopCount, RDX					; | 
			jnz loop2							; | if loopCount == 0 break

pshufd xmm7, xmm7, 1
movd EAX, xmm7					    ; | bylo movq
mov [R9], EAX						; | resultRow = rows * columns
add R9, 4							; | resultRows++ 4 co 8 el bylo 4

mov RDX, secondLoopCount			; |
dec RDX								; |
mov secondLoopCount, RDX			; |
jnz loop1							; | if secondLoopCount == 0 break


ret
AsmMultiplication ENDP


end
 */ 