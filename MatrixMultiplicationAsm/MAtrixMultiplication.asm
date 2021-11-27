.CODE

;-------------------------------------------------------------------------
;-------------------------------------------------------------------------

AsmMultiplication PROC outerLoopCount: dword, innerLoopCount: dword, startColAddress : qword, startRowAddress : qword, count : qword, columns : dword     

													; resultRow in RCX
													; rowToMultiply in RDX
													; colToMultiply in R8
													; argsPtr in R9


mov EAX, [R9]
mov outerLoopCount, EAX
mov columns, EAX

mov EAX, [R9 + 4]
mov innerLoopCount, EAX

mov count, 0

mov R10, RDX
mov R9, RCX

mov startColAddress, R8
mov startRowAddress, R10

loop1:
mov R10, startRowAddress								; | row = startRowAddress

xor RAX, RAX											; |
mov [R9], EAX											; | (*resultRow) = 0 RAX

mov ECX, innerLoopCount									; |
pxor xmm2, xmm2											; | preparing for multiplying in loop2
inc count
	
			loop2:
			movq xmm0, qword ptr [R10]					; | move actual row element to vector
			movq xmm4, qword ptr [R8]					; | move actual column element to vector
			pmulld xmm0, xmm4							; | multiply vectors
			pxor xmm3, xmm3
			haddps xmm3, xmm0
			paddq xmm2, xmm3							; | add result to third vector

			add R10, 8									; | row++ 4
			add R8, 8									; | columns ++ 4


			dec ECX										; | decrementing loop counter
			dec ECX
			jnz loop2									; | if loopCount == 0 break

pshufd xmm2, xmm2, 2
movq RAX, xmm2											; |
mov [R9], EAX											; | resultRow = rows * columns EAX
add R9, 4												; | resultRows++ 4


mov EDX, outerLoopCount									; |
dec EDX													; |
mov outerLoopCount, EDX									; |
jnz loop1												; | if secondLoopCount == 0 break


ret
AsmMultiplication ENDP


end