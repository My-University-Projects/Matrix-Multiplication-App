 .CODE

;-------------------------------------------------------------------------
;-------------------------------------------------------------------------

AsmMultiplication PROC loopCount: qword, secondLoopCount: qword, startColAddress : qword, startRowAddress : qword, count : qword, matrixSize : qword                                                                                                                                                               
						; resultRow in RCX
						; rowToMultiply in RDX
						; colToMultiply in R8
						; size int R9
mov matrixSize, R9
mov loopCount, R9
mov secondLoopCount, R9
mov count, 0

mov R10, RDX
mov R9, RCX

mov startColAddress, R8
mov startRowAddress, R10

loop1:
mov R8, startColAddress				; column = startColAddress
mov R10, startRowAddress			; row = startRowAddress

mov RAX, count						; |
;mov RCX, 4							; |
;mul RCX							; |
shl RAX, 2							; |
add R8, RAX							; |	column += i

xor RAX, RAX						; |
mov [R9], EAX						; (*resultRow) = 0 RAX

mov RAX, matrixSize					; |
mov loopCount, RAX					; |
pxor xmm2, xmm2						; | preparing for multiplying in loop2
inc count
	
			loop2:
			movq xmm0, qword ptr [R10]			;move actual row element to vector
			movq xmm4, qword ptr [R8]			;move actual column element to vector
			pmulld xmm0, xmm4					;multiply vectors
			paddq xmm2, xmm0					;add result to third vector

			add R10, 4							; row++ 4

			mov RAX, matrixSize					; |
			;mov RDX, 4							; |
			;mul RDX
			shl RAX, 2
			add R8, RAX							; | column += size

			mov RDX, loopCount					; |
			dec RDX								; | decrementing loop counter
			mov loopCount, RDX					; | 
			jnz loop2							; | if loopCount == 0 break

movq RAX, xmm2						; |
mov [R9], EAX						; | resultRow = rows * columns
add R9, 4							; | resultRows++ 4

mov RDX, secondLoopCount			; |
dec RDX								; |
mov secondLoopCount, RDX			; |
jnz loop1							; | if secondLoopCount == 0 break


ret
AsmMultiplication ENDP


end