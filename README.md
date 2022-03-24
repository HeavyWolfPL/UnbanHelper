[![Exiled Framework](https://cdn.discordapp.com/attachments/880982483213111356/956486176523571210/developed-using-exiled-5.0.svg)](https://github.com/Exiled-Team/EXILED)[![SCP: Secret Laboratory](https://cdn.discordapp.com/attachments/880982483213111356/880984656705630238/for_-scp_-secret-laboratory.svg)](https://scpslgame.com/)

# UnbanHelper
Handle unbans more easily.

## Commands

| Command | Parameters | Permissions | Aliases |
| --- | --- | --- | --- |
| unbanid | Steam/Discord ID | unbanhelper.id | uid, gunban |
| asncheck | RA ID | unbanhelper.asncheck | None |

## Config
```yaml
UH:
  is_enabled: true
  # List of ASN Addresses that are whitelisted from being bans. Check the readme for more info
  asn_anti_ban:
  - 50889
  debug: false
```
### What is that ASN Anti-Ban?
Well, it's a whitelist for ASN addresses. By default only one ASN is whitelisted, it belongs to GeForce Now Service.
<br>ASN Anti-Ban does not mean a person won't be banned at all, only their SteamID/Discord ID will be banned, not IP.
<br>If you still don't understand how it works, deal with it.

---

### To-Do
- Unban command with IP as an argument