# Project Useful  

* Research of new, useful ideas that can be applied to everyday work.  
* Learning and applying new technologies.  

Disclaimer!  The aim of this project is for learning.  As a result, the UIs aren't shiny, there may be bugs and features may be incomplete.  However all the bugs found are fixed and the UIs are only polished to the point of making them usable.  

## Cryptography  

### Usage  
- Ciphers have an implementation class and a class that inherits from SymmetricAlgorithm.  
- Key and IV values are in Unicode.

#### [Atbash](https://en.wikipedia.org/wiki/Atbash)  
No settings required.  

#### [Caesar](https://en.wikipedia.org/wiki/Caesar_cipher)  
Settings:  
```
key={RightShift}  
iv=  
```
Default:  
```
key=01  
iv=  
```

#### [Enigma M3](https://en.wikipedia.org/wiki/Enigma_machine)  
Settings:  
```
key={reflector number|rotor order|ring setting|plugboard setting}  
iv={rotor setting}
```
Default:  
```
key=B|III II I|01 01 01|  
iv=01 01 01  
```
Example: 
```
key=B|III II I|03 02 01|BJ DN FW GR HY IS KC PV QX TM  
```

#### [MonoAlphabetic](https://en.wikipedia.org/wiki/Substitution_cipher)
Settings:  
```
key={character set|substitutions}  
iv=  
```
Default:  
```
key=ABCDEFGHIJKLMNOPQRSTUVWXYZ|ABCDEFGHIJKLMNOPQRSTUVWXYZ  
iv=  
```
Example: 
```
key=ABCD|BCDA  
```

#### [Reflector](https://en.wikipedia.org/wiki/Substitution_cipher)
Settings:  
```
key={character set|substitutions}  
iv=  
```
Default:  
```
key=ABCDEFGHIJKLMNOPQRSTUVWXYZ|ABCDEFGHIJKLMNOPQRSTUVWXYZ
iv=  
```
Example: 
```
key=ABCD|BADC  
```

#### [ROT13](https://en.wikipedia.org/wiki/ROT13)
No settings required.  
